// #region File Annotation
// 
// Author：Zhiqiang Li
// 
// FileName：OrderController.cs
// 
// Project：RedisLock.AspNetCore
// 
// CreateDate：2018/04/26
// 
// Note: The reference to this document code must not delete this note, and indicate the source!
// 
// #endregion

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RedisLock.AspNetCore.Cache;
using RedisLock.AspNetCore.Config;
using RedisLock.AspNetCore.Model;

namespace RedisLock.AspNetCore.Controllers
{
	[Route("[controller]")]
	public class OrderController : Controller
	{
		private IXcCache _cache;
		private ILogger _logger;
		public OrderController(IXcCache cache,ILogger<OrderController> logger)
		{
			_cache = cache;
			_logger = logger;
		}

		[HttpPost("[action]")]
		public async Task<JsonResult> Create([FromBody] CreateOrderDto dto)
		{
			var lockVal = $"{Environment.CurrentManagedThreadId}.{Environment.MachineName}";
			var lockTake = await _cache.LockTakeAsync(string.Format(AppCacheKey.ProductLock, dto.ProductId), lockVal, TimeSpan.FromMinutes(2));

			var startTime = DateTime.Now;
			var retryCount = 0;
			while (!lockTake)
			{
				if ((DateTime.Now - startTime).TotalSeconds > 5)
				{
					break;
				}

				retryCount++;
				lockTake = await _cache.LockTakeAsync(string.Format(AppCacheKey.ProductLock, dto.ProductId), lockVal, TimeSpan.FromMinutes(2));

				_logger.LogWarning($"UserId:{dto.UserId}.Lock take failed again.Retry Count：{retryCount}");

				Thread.Sleep(100);
			}
			GC.Collect();
			if (lockTake)
			{
				_logger.LogInformation($"UserId:{dto.UserId}.Lock take success.");
			}
			else
			{
				_logger.LogWarning($"UserId:{dto.UserId}.Lock take failed.");
			}


			return Json(new XcHttpResult(){Msg = lockTake? "Lock take success" : "Lock take failed", Result = lockTake });
		}
	}
}