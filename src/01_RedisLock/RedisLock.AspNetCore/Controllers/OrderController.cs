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
using RedisLock.AspNetCore.Extensions;
using RedisLock.AspNetCore.Model;

namespace RedisLock.AspNetCore.Controllers
{
	[Route("[controller]")]
	public class OrderController : Controller
	{
		private IXcCache _cache;
		private ILogger _logger;
		private LockProcessor.LockProcessor _lockProcessor;
		public OrderController(IXcCache cache,ILogger<OrderController> logger, LockProcessor.LockProcessor lockProcessor)
		{
			_cache = cache;
			_logger = logger;
		    _lockProcessor = lockProcessor;
		}

		[HttpPost("[action]")]
		public async Task<JsonResult> Create([FromBody] CreateOrderDto dto)
		{
		    string lockKey = string.Format(AppCacheKey.ProductLock, "xxxxxxx");

		    var result = await _lockProcessor.ExecuteAsync(() =>
		    {
                //
		    }, lockKey);
            
            return Json(new XcHttpResult(){Msg = result ? "Execute success" : "Execute failed", Result = result });
		}
	}
}