using System;
using System.Net;

namespace Core.Exceptions
{
    public class LateReportedTransactionException : Exception
    {
        public HttpStatusCode ErrorStatusCode = HttpStatusCode.NoContent;
    }
}