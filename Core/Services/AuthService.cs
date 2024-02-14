using Contracts.DtoModels;
using Contracts.ReqModels;
using Contracts.ResModels;
using Domain;
using Domain.Enums;
using Services.Abstractions;
using Contracts.Mapper;
using Domain.Extensions;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Contracts.ConfigModels;
using System.Threading;
using Contracts.Extensions;

namespace Services.Services
{
	public class AuthService : ReqServiceBase, IAuthService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IConfiguration _config;

		//private readonly SignInManager<ApplicationUser> _signInManager;
		//private readonly ICurrentUser currentUser;
		public AuthService(IUnitOfWork unitOfWork, ICurrentUser currentUser,
			UserManager<ApplicationUser> userManager,
			RoleManager<IdentityRole> roleManager, IConfiguration configuration) :
			base(unitOfWork, currentUser)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			this._config = configuration;
		}
		public UserManager<ApplicationUser> UserManager => _userManager;
		public RoleManager<IdentityRole> RoleManager => _roleManager;

		public async Task<IResBase<AuthTokenRes>> Authenticate(AuthLoginReq req)
		{
			var jwt = _config.GetJwtSettingConfiguration();
			
			var user = await _userManager.FindByNameAsync(req.Username);
			if (user != null && await _userManager.CheckPasswordAsync(user, req.Password))
			{
				var userRoles = await _userManager.GetRolesAsync(user);
				//var role = userRoles.FirstOrDefault();
				var authClaims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, user.UserName),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
					new Claim(JwtRegisteredClaimNames.NameId, user.Id),
					//new Claim("Name", req.Username),
					new Claim(JwtRegisteredClaimNames.Email, user.Email),
					//new Claim("Role", role),
				};

				foreach (var userRole in userRoles)
				{
					authClaims.Add(new Claim(ClaimTypes.Role, userRole));
				}

				var token = CreateToken(authClaims);
				var refreshToken = GenerateRefreshToken();

				var refreshTokenValidityInDays = jwt.RefreshTokenValidityInDays;
				user.RefreshToken = refreshToken;
				user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

				await _userManager.UpdateAsync(user);

				var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
				return ResBase.SuccessRes(new AuthTokenRes
				{
					Token = jwtToken,
					RefreshToken = refreshToken,
					Expiration = token.ValidTo
				});
			}
			return ResBase.ErrorRes<AuthTokenRes>("Incorrect authentication.");
		}
		public async Task<IResBase<ApiTokenRes>> RefreshToken(ApiTokenReq req)
		{
			if (req is null)
			{
				return ResBase.ErrorRes<ApiTokenRes>("Invalid request");
			}

			string? accessToken = req.AccessToken;
			string? refreshToken = req.RefreshToken;

			var principal = GetPrincipalFromExpiredToken(accessToken);
			if (principal == null)
			{
				return ResBase.ErrorRes<ApiTokenRes>("Invalid access token or refresh token");
			}
			string username = principal.Identity.Name;

			var user = await _userManager.FindByNameAsync(username);

			if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
			{
				return ResBase.ErrorRes<ApiTokenRes>("Invalid access token or refresh token");
			}

			var newAccessToken = CreateToken(principal.Claims.ToList());
			var newRefreshToken = GenerateRefreshToken();

			user.RefreshToken = newRefreshToken;
			await _userManager.UpdateAsync(user);

			return ResBase.SuccessRes(new ApiTokenRes
			{
				AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
				RefreshToken = newRefreshToken
			});
		}
		public async Task<IResBase> RevokeToken(string username)
		{
			var user = await _userManager.FindByNameAsync(username);
			if (user == null) return ResBase.ErrorRes("Invalid user name");

			user.RefreshToken = null;
			await _userManager.UpdateAsync(user);

			return ResBase.SuccessRes();
		}
		
		//helpers

		private static string GenerateRefreshToken()
		{
			var randomNumber = new byte[64];
			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);
			return Convert.ToBase64String(randomNumber);
		}
		private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
		{
			var jwt = _config.GetJwtSettingConfiguration();
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateAudience = false,
				ValidateIssuer = false,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Secret)),
				ValidateLifetime = false
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
			if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
				throw new SecurityTokenException("Invalid token");

			return principal;

		}
		private JwtSecurityToken CreateToken(List<Claim> authClaims)
		{
			var jwt = _config.GetJwtSettingConfiguration();

			var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Secret));
			
			var token = new JwtSecurityToken(
				issuer: jwt.ValidIssuer,
				audience: jwt.ValidAudience,
				expires: DateTime.Now.AddMinutes(jwt.TokenValidityInMinutes),
				claims: authClaims,
				signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
				);

			return token;
		}
	}
}
