using Contracts.ReqModels;
using Contracts.ResModels;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Services.Abstractions
{
	public interface IAuthService : IReqServiceBase
	{
		UserManager<ApplicationUser> UserManager { get; }
		RoleManager<IdentityRole> RoleManager { get; }
		
		Task<IResBase<AuthTokenRes>> Authenticate(AuthLoginReq req);
		Task<IResBase> RevokeToken(string username);
		Task<IResBase<ApiTokenRes>> RefreshToken(ApiTokenReq req);

	}
}
