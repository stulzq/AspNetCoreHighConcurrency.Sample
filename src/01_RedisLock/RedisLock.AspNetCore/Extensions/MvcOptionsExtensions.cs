// #region File Annotation
// 
// Author：Zhiqiang Li
// 
// FileName：MvcOptionsExtensions.cs
// 
// Project：RedisLock.AspNetCore
// 
// CreateDate：2018/04/26
// 
// Note: The reference to this document code must not delete this note, and indicate the source!
// 
// #endregion

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace RedisLock.AspNetCore.Extensions
{
	/// <summary>
	/// code from https://www.cnblogs.com/savorboard/p/dontnet-IApplicationModelConvention.html
	/// </summary>
	public static class MvcOptionsExtensions
	{
		public static void UseCentralRoutePrefix(this MvcOptions opts, string route)
		{
			// 添加我们自定义 实现IApplicationModelConvention的RouteConvention
			opts.Conventions.Insert(0, new RouteConvention(new RouteAttribute(route)));
		}
	}
}