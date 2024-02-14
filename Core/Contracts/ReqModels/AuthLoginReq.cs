using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Contracts.DtoModels;
using Contracts.ReqModels.Base;

namespace Contracts.ReqModels
{
	public class AuthLoginReq 
	{
		//public AuthLoginReq(string username, string pwd)
		//{
		//	this.Username = username;
		//	this.Password = pwd;
		//}
		public string Username { get; set; }
		public string Password { get; set; }
	}
	
}
