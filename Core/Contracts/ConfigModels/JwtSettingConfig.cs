using Contracts.ConfigModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.ConfigModels
{
	public class JwtSettingConfig
	{
		public string Secret { get; set; }
		public string ValidIssuer { get; set; }
		public string ValidAudience { get; set; }
		public int TokenValidityInMinutes { get; set; }
		public int RefreshTokenValidityInDays { get; set; }
	}
}
