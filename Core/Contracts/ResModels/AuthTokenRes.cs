using Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Contracts.ResModels
{
	public class AuthTokenRes
	{
		public string Token { get; set; }
		public string RefreshToken { get; set; }
		public DateTime Expiration { get; set; }
	}
}
