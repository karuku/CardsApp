using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Exceptions
{
	public sealed class ServerConnectionException: Exception
	{
		public ServerConnectionException() : base("Unable to connect to database")
		{

		}
		public ServerConnectionException(string message) : base(message)
		{

		}
	}
}
