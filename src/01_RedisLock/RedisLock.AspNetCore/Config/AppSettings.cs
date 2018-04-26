// #region File Annotation
// 
// Author：Zhiqiang Li
// 
// FileName：AppSettings.cs
// 
// Project：RedisLock.AspNetCore
// 
// CreateDate：2018/04/26

// Note: The reference to this document code must not delete this note, and indicate the source!
// 
// #endregion

using Microsoft.Extensions.Configuration;

namespace RedisLock.AspNetCore.Config
{
	public class AppSettings
	{
		public IConfiguration Configuration { get; }

		public AppSettings(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public string RedisConnectionString => Configuration.GetConnectionString("Redis");
	}
}