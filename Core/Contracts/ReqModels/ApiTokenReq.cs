using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Contracts.ResModels
{
	public class ApiTokenReq
	{
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
	}
}
