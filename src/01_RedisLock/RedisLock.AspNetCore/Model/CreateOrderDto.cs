// #region File Annotation
// 
// Author：Zhiqiang Li
// 
// FileName：CreateOrderDto.cs
// 
// Project：RedisLock.AspNetCore
// 
// CreateDate：2018/04/26
// 
// Note: The reference to this document code must not delete this note, and indicate the source!
// 
// #endregion

using System.ComponentModel.DataAnnotations;

namespace RedisLock.AspNetCore.Model
{
	public class CreateOrderDto
	{
		[Required(ErrorMessage = "商品ID不得为空")]
		public string ProductId { get; set; }

		[Required(ErrorMessage = "购买商品数量不得为空")]
		[Range(1,999,ErrorMessage = "购买商品数量必须介于1~999之间")]
		public int Number { get; set; } = 0;

		[Required(ErrorMessage = "用户ID不得为空")]
		public string UserId { get; set; }
	}
}