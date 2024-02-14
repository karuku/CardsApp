using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Contracts.ResModels
{
	public class AuthRes
	{
		public AuthRes(string token)
		{
			Token = token;
		}
		public string Token { get; set; }
	}
	public class AuthUserRes
	{
		public SysRoleTypes SysRole { get; set; }
		public string UserId { get; set; }
		[StringLength(200)]
		public string UserName { get; set; }
		[StringLength(200)]
		public string Email { get; set; }
	}
}
