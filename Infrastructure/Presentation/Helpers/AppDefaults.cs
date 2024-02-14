using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Helpers
{
	public class AppDefaults
	{
		public const string DefaultUserPassword = "User1234!";

		public static IEnumerable<ApplicationUser> DefaultUsers = new List<ApplicationUser>
		{
			new ApplicationUser("admin@logicea.com")
			{
				Email="admin@logicea.com",
			},
			new ApplicationUser("member@logicea.com")
			{
				Email="member@logicea.com",
			}
		};
	}
}
