using Contracts.ReqModels.Base;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Contracts.ReqModels
{
    public class UserReq : ReqBase
	{
		
	}
	
	public class EditUserReq : UpdateReqBase
	{
		[StringLength(200)]
		public string UserName { get; set; }
		[StringLength(200)]
		public string Password { get; set; }
		[StringLength(200)]
		public string Email { get; set; }
		public SysRoleTypes Role { get; set; }
	}
}
