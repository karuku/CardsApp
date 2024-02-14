using Asp.Versioning;
using Contracts.Extensions;
using Contracts.Mapper;
using Contracts.ReqModels;
using Contracts.ResModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Presentation.Extensions;
using Services.Abstractions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CardsApis.Controllers
{
	[Route("api/v{version:apiVersion}/oauth")]
	//[Route("api/oauth/v1")]
	public class AuthorizationController : ApiBaseController
	{
		private readonly IServiceManager _serviceManager;
		private readonly ILogger<AuthorizationController> _logger;
		private IConfiguration _config;
		public AuthorizationController(IServiceManager serviceManager, 
			ILogger<AuthorizationController> logger, IConfiguration config)
		{
			_serviceManager = serviceManager;
			_logger = logger;
			_config = config;
		}

		/// <summary>
		/// Post request to authenticate user and get a time bound token for API authentication.
		/// </summary>
		/// <param name="req"></param>
		/// <param name="cancellationtoken"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPost("authenticate")]
		public async Task<IActionResult> Authenticate(AuthLoginReq req, CancellationToken cancellationtoken)
		{
			//your logic for login process
			//If login username and password are correct then proceed to generate token
			var dataRes = await _serviceManager.AuthService.Authenticate(req);

			if (!dataRes.IsSuccessful() || dataRes.Datas==null) return BadRequest(dataRes);
			
			return Ok(dataRes);
		}
		/// <summary>
		/// Post request to refresh a time bound access token for Api authentication
		/// with an active refresh token and the expired access token.
		/// </summary>
		/// <param name="req">object with an active refresh token and the expired access token. </param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPost]
		[Route("refreshToken")]
		public async Task<IActionResult> RefreshToken(ApiTokenReq req)
		{
			var res = await _serviceManager.AuthService.RefreshToken(req);
			if (!res.IsSuccessful())
			{
				return BadRequest(res.Message);
			}

			return new ObjectResult(res.Datas);
		}

		/// <summary>
		/// Post request to revoke a time bound refresh token.
		/// Once revoked, the refresh token can no longer be used to refresh an access token.
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("revokeToken/{username}")]
		public async Task<IActionResult> RevokeToken(string username)
		{
			if (username.IsNullOrWhiteSpace()) return BadRequest(ResBase.ErrorRes("Username is required"));
			
			var res = await _serviceManager.AuthService.RevokeToken(username);
			if (!res.IsSuccessful()) return BadRequest(res.Message);

			return NoContent();
		}
	}
}
