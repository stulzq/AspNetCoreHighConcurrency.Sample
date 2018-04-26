// #region File Annotation
// 
// Author：Zhiqiang Li
// 
// FileName：XcActionFilter.cs
// 
// Project：RedisLock.AspNetCore
// 
// CreateDate：2018/04/27
// 
// Note: The reference to this document code must not delete this note, and indicate the source!
// 
// #endregion

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RedisLock.AspNetCore.Filter
{
	public class XcActionFilter: IActionFilter
	{
		public void OnActionExecuting(ActionExecutingContext context)
		{
			if (!context.ModelState.IsValid)
			{
				XcHttpResult result=new XcHttpResult(){Result = false};

				foreach (var item in context.ModelState.Values)
				{
					foreach (var error in item.Errors)
					{
						result.Msg += error.ErrorMessage+"|";
					}
				}

				context.Result=new JsonResult(result);
			}
		}

		public void OnActionExecuted(ActionExecutedContext context)
		{
			
		}
	}
}