using System;
using System.Net;

namespace Core.Exceptions
{
    public class UnprocessableTransactionException : Exception
    {
        public HttpStatusCode ErrorStatusCode = HttpStatusCode.UnprocessableEntity;
    }
}