using Microsoft.AspNetCore.Identity;
using System;

namespace Domain.Entities
{
	public class ApplicationUser : IdentityUser
	{
		public ApplicationUser() : base()
		{

		}
		public ApplicationUser(string username) : base(username)
		{

		}

		public string? RefreshToken { get; set; }
		public DateTime RefreshTokenExpiryTime { get; set; }
	}
	
}
