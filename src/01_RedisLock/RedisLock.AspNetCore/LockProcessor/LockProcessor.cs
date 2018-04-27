using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RedisLock.AspNetCore.Cache;

namespace RedisLock.AspNetCore.LockProcessor
{
    public class LockProcessor
    {
        private readonly ILogger _logger;
        private readonly IXcCache _cache;

        public LockProcessor(ILogger<LockProcessor> logger, IXcCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        /// <summary>
        /// 使用分布式锁执行业务 此方法返回的结果为业务是否执行成功，而不表示获取锁或者释放锁的成功
        /// </summary>
        /// <param name="action"></param>
        /// <param name="lockKey"></param>
        /// <returns>true 业务执行成功，反之则失败</returns>
        public async Task<bool> ExecuteAsync(Action action,string lockKey)
        {
            var result = false;

            //生成锁的值，以便解锁时使用；禁止在解锁的时候再次动态拼接，因为await执行完毕以后线程id会变
            var lockVal = $"{Environment.CurrentManagedThreadId}.{Environment.MachineName}";

            //首次尝试获取锁 过期时间 10s
            var lockTake = await _cache.LockTakeAsync(lockKey, lockVal, TimeSpan.FromSeconds(10));

            var startTime = DateTime.Now;
            var retryCount = 0;
            while (!lockTake)
            {
                //判断是否超时
                if ((DateTime.Now - startTime).TotalSeconds > 5)
                {
                    break;
                }

                retryCount++;
                //再次尝试获取锁
                lockTake = await _cache.LockTakeAsync(lockKey, lockVal, TimeSpan.FromMinutes(2));

                _logger.LogWarning($"Lock take failed again.Retry Count：{retryCount}.Key:{lockKey}.Value:{lockVal}");

                Thread.Sleep(100);
            }

            //进行高频率循环以后最好调用GC强制回收
            GC.Collect();

            if (lockTake)
            {
                _logger.LogInformation($"Lock take success.Key:{lockKey}.Value:{lockVal}");

                //do something

                try
                {
                    action();
                    result = true;
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Lock processor error.Key:{lockKey}.Value:{lockVal}");
                }
                
            }
            else
            {
                _logger.LogWarning($"Lock take failed.Key:{lockKey}.Value:{lockVal}");
            }

            //解锁
            var lockRelease = await _cache.LockReleaseAsync(lockKey, lockVal);
            if (lockRelease)
            {
                _logger.LogInformation("Lock release success.Key:{lockKey}.Value:{lockVal}");
            }
            else
            {
                _logger.LogWarning($"Lock release failed.Key:{lockKey}.Value:{lockVal}");
            }

            return result;
        }
    }
}