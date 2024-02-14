using Domain.Exceptions;
using System;

namespace Persistence.Exceptions
{
    public sealed class InvalidDatabaseServerException : BadRequestException
	{
        public InvalidDatabaseServerException(string message)
            : base($"The database server is not valid. Error:{message}")
        {
        }
    }
}
