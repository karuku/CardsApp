using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
	public interface ICurrentUser
	{
		public string UserId { get; set; }
		public string UserName { get; set; }
		public string Role { get; set; }
	}
}
