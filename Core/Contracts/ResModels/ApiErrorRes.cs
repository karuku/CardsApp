using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Contracts.ResModels
{
	public class ApiErrorRes
	{
		public int StatusCode { get; set; }
		public string Message { get; set; }
	}
}
