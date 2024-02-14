using Newtonsoft.Json;
using Contracts.DtoModels;
using Contracts.ReqModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Contracts.ResModels
{

	public interface IResBase
	{
		string Message { get; set; }
		int ResCode { get; set; }
	}

	public interface IResBase<T> : IResBase //where T : class
	{
		T Datas { get; set; }
		//string Message { get; set; }
		//byte ResCode { get; set; }
	}
	
	public class ResBase : IResBase
	{
		public int ResCode { get; set; }
		public string Message { get; set; }

		public static IResBase Res(int code,string message = null)
		{
			return new ResBase
			{
				ResCode = code,
				Message = message
			};
		}
		public static IResBase DefaultRes(string message = null)
		{
			return Res(1, message ?? "Default Res");
		}
		public static IResBase SuccessRes(string message = null)
		{
			return Res(0, message ?? "Success");
		}
		public static IResBase SuccessAddRes(string message = null)
		{
			return Res(0, message ?? "Added Successfully");
		}
		public static IResBase<T> SuccessAddRes<T>(T datas, string message = null) //where T : class
		{
			var res = string.Format($"Success Add: {message}");
			return Res<T>(0, datas, message ?? res);
		}
		public static IResBase SuccessUpdateRes(string message = null)
		{
			return Res(0, message ?? "Updated Successfully");
		}
		public static IResBase<T> SuccessUpdateRes<T>(T datas, string message = null) //where T : class
		{
			var res = string.Format($"Success Update: {message}");
			return Res<T>(0, datas, message ?? res);
		}
		public static IResBase SuccessDeleteRes(string message = null)
		{
			return Res(0, message ?? "Deleted Successfully");
		}

		public static IResBase ErrorRes(string message = null)
		{
			//var res = string.Format($"App Error Ocurred: {message}");
			var res = $"App Error Ocurred: {message}";
			return Res(1, res);
		}
		public static IResBase InvalidRes(string message = null)
		{
			var res = string.Format($"Invalid data: {message}");
			return Res(1, res);
		}
		public static IResBase NotFoundRes(string message = null)
		{
			var res = string.Format($"Error, Data not found: {message}");
			return Res(1, res);
		}
		public static IResBase ExistsRes(string message = null)
		{
			var res = string.Format($"Error, Record already exists: {message}");
			return Res(1, res);
		}

		public static IResBase<T> Res<T>(int code,T data, string message = null) //where T : class
		{
			return new ResBase<T>
			{
				ResCode = code,
				Message = message ?? "Result",
				Datas = data
			};
		}
		public static IResBase<T> SuccessRes<T>(T datas, string message = null)// //where T : class
		{
			var res = string.Format($"Success: {message}");
			return Res<T>(0, datas, message ?? res);
		}
		public static IResBase<T> ErrorRes<T>(string message = null) //where T : class
		{
			var res = string.Format($"App Error Ocurred: {message}");
			return Res<T>(1, default, message ?? res);
		}
		public static IResBase<T> NotFoundRes<T>(string message = null) //where T : class
		{
			var res = string.Format($"Error, Data not found: {message}");
			return Res<T>(1, default, res);
		}
		public static IResBase<T> ExistsRes<T>(string message = null) //where T : class
		{
			var res = string.Format($"Error, Record already exists: {message}");
			return Res<T>(1, default, res);
		}
		public static IResBase<T> UnAuthorized<T>(string message = null) //where T : class
		{
			var res = string.Format($"UnAuthorized Access");
			return Res<T>(401, default, res);
		}

	}

	public class ResBase<T> : ResBase, IResBase<T> //where T : class
	{
		//public byte ResCode { get; set; }
		//public string Message { get; set; }
		public T Datas { get; set; }

		public static IResBase<T> Res(T data,string message = null)
		{
			return new ResBase<T>
			{
				ResCode = 0,
				Message = message ?? "Result",
				Datas= data
			};
		}
		public static IResBase<T> SuccessRes(T data, string message = null)
		{
			return new ResBase<T>
			{
				ResCode = 0,
				Message = message ?? "Success",
				Datas = data
			};
		}
		public static IResBase<T> ErrorRes(T data=default, string message = null)
		{
			return new ResBase<T>
			{
				ResCode = 0,
				Message = message ?? "Error",
				Datas = data
			};
		}

	}

	public class ApiResBase
	{
		public ApiResBase() { }
		public ApiResBase(IResBase baseRes = default)
		{
			Status = baseRes;
		}

		[JsonProperty(Order = -2)]
		//public IResBase Status { get; protected set; }
		public IResBase Status { get; protected set; }
	}

	public class ApiResBase<T> : ApiResBase
	//where T : class
	{
		public ApiResBase() : base()
		{
		}
		public ApiResBase(IResBase baseRes) : base(baseRes)
		{
		}

		[JsonProperty("data", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
		public T Data { get; set; } = default;

		public static ApiResBase<T> SuccessRes(T data, string message = null)
		{
			return new ApiResBase<T>()
			{
				Data = data ?? default,
				//Data = data?? (T)Activator.CreateInstance(typeof(T)),
				Status = ResBase.SuccessRes(message)
			};
		}
		public static ApiResBase<T> ErrorRes(string message = null)
		{
			return new ApiResBase<T>()
			{
				Data = default,
				//Data = (T)Activator.CreateInstance(typeof(T)),
				Status = ResBase.ErrorRes(message)
			};
		}
		public static ApiResBase<T> ErrorRes(T data, string message = null)
		{
			return new ApiResBase<T>()
			{
				Data = data ?? default,
				//Data = (T)Activator.CreateInstance(typeof(T)),
				Status = ResBase.ErrorRes(message)
			};
		}
	}

	public class ApiListResBase<T> : ApiResBase
	//where T : class//,new()
	{
		public ApiListResBase()
				  : base() { }
		public ApiListResBase(IResBase baseRes)
						  : base(baseRes) { }

		public IEnumerable<T> Datas { get; set; } = null;
		[JsonProperty(Order = -2)]
		public long RecordCount { get; set; } = 0;

		public static ApiListResBase<T> SuccessRes(IEnumerable<T> data, long totalRecords, string message = null)
		{
			return new ApiListResBase<T>()
			{
				Datas = data,
				RecordCount = totalRecords,
				Status = ResBase.Res(0, message)
			};
		}
		public static ApiListResBase<T> EmptyRes(string message = null)
		{
			return new ApiListResBase<T>()
			{
				Datas = default,
				RecordCount = 0,
				Status = ResBase.Res(1, message)
			};
		}
		public static ApiListResBase<T> ErrorRes(string message = null)
		{
			return new ApiListResBase<T>()
			{
				Datas = default,
				RecordCount = 0,
				Status = ResBase.Res(1, message)
			};
		}

	}

}
