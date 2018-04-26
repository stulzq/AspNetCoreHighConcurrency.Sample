// #region File Annotation
// 
// Author：Zhiqiang Li
// 
// FileName：XcRedisCache.cs
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
using Microsoft.Extensions.Logging;
using RedisLock.AspNetCore.Config;
using StackExchange.Redis;

namespace RedisLock.AspNetCore.Cache
{
	public class XcRedisCache:IXcCache,IDisposable
	{
		private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(1, 1);
		private volatile ConnectionMultiplexer _connection;
		private IDatabase _cache;
		private ILogger _logger;
		private readonly AppSettings _settings;

		public XcRedisCache(AppSettings settings,ILogger<XcRedisCache> logger)
		{
			_settings = settings;
			_logger = logger;
		}

		private async Task ConnectAsync(CancellationToken token = default(CancellationToken))
		{
			token.ThrowIfCancellationRequested();

			if (_connection != null)
			{
				return;
			}

			await _connectionLock.WaitAsync(token);
			try
			{
				if (_connection == null)
				{
					_connection = await ConnectionMultiplexer.ConnectAsync(_settings.RedisConnectionString);
					_cache = _connection.GetDatabase();
					_logger.LogInformation("Redis connect sucess.");
				}
			}
			finally
			{
				_connectionLock.Release();
			}
		}

		public async Task<bool> LockTakeAsync(string key, string value, TimeSpan expiry)
		{
			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			if (expiry == null)
			{
				throw new ArgumentNullException(nameof(expiry));
			}

			await ConnectAsync();

			return await _cache.LockTakeAsync(key, value, expiry);
		}

		public async Task<bool> LockReleaseAsync(string key, string value)
		{
			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			await ConnectAsync();

			return await _cache.LockReleaseAsync(key, value);
		}


		public void Dispose()
		{
			_connection?.Close();
		}
	}
}