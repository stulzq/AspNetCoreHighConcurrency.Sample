// #region File Annotation
// 
// Author：Zhiqiang Li
// 
// FileName：IXcCache.cs
// 
// Project：RedisLock.AspNetCore
// 
// CreateDate：2018/04/26
// 
// Note: The reference to this document code must not delete this note, and indicate the source!
// 
// #endregion

using System;
using System.Threading.Tasks;

namespace RedisLock.AspNetCore.Cache
{
	public interface IXcCache
	{
		/// <summary>
		/// 获取锁
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="expiry"></param>
		/// <returns></returns>
		Task<bool> LockTakeAsync(string key, string value, TimeSpan expiry);

		/// <summary>
		/// 释放锁
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		Task<bool> LockReleaseAsync(string key, string value);
	}
}