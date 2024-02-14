using System;

namespace Domain.Exceptions
{
    public sealed class UnAuthorizedUserException : UnAuthorizedException
    {
        public UnAuthorizedUserException()
            : base($"UnAuthorized access")
        {
        }
    }
    public abstract class UnAuthorizedException : Exception
    {
        public UnAuthorizedException(string message)
            : base(message)
        {
        }
    }
}