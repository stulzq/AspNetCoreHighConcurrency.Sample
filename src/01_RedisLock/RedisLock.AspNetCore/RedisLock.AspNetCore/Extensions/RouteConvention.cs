// #region File Annotation
// 
// Author：Zhiqiang Li
// 
// FileName：RouteConvention.cs
// 
// Project：RedisLock.AspNetCore
// 
// CreateDate：2018/04/26
// 
// Note: The reference to this document code must not delete this note, and indicate the source!
// 
// #endregion

using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;

namespace RedisLock.AspNetCore.Extensions
{
	/// <summary>
	/// code from https://www.cnblogs.com/savorboard/p/dontnet-IApplicationModelConvention.html
	/// </summary>
	public class RouteConvention:IApplicationModelConvention
	{
		private readonly AttributeRouteModel _centralPrefix;

		public RouteConvention(IRouteTemplateProvider routeTemplateProvider)
		{
			_centralPrefix = new AttributeRouteModel(routeTemplateProvider);
		}

		//接口的Apply方法
		public void Apply(ApplicationModel application)
		{
			//遍历所有的 Controller
			foreach (var controller in application.Controllers)
			{
				// 已经标记了 RouteAttribute 的 Controller
				var matchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel != null).ToList();
				if (matchedSelectors.Any())
				{
					foreach (var selectorModel in matchedSelectors)
					{
						// 在 当前路由上 再 添加一个 路由前缀
						selectorModel.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_centralPrefix,
							selectorModel.AttributeRouteModel);
					}
				}

				// 没有标记 RouteAttribute 的 Controller
				var unmatchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel == null).ToList();
				if (unmatchedSelectors.Any())
				{
					foreach (var selectorModel in unmatchedSelectors)
					{
						// 添加一个 路由前缀
						selectorModel.AttributeRouteModel = _centralPrefix;
					}
				}
			}
		}
	}
}