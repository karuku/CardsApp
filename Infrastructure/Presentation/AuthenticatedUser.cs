using Domain;
using Domain.Enums;

namespace Presentation
{
	public class AuthenticatedUser : ICurrentUser
	{
		public string UserName { get; set; } 

		public string Role { get; set; }
		public string UserId { get; set; }
	}
}
