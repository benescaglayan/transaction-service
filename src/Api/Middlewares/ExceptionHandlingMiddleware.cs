using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;
using Core.Exceptions;

namespace TransactionService.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                if (e is UnprocessableTransactionException unprocessableTransactionException)
                {
                    httpContext.Response.StatusCode = (int) unprocessableTransactionException.ErrorStatusCode;
                }
                else if (e is LateReportedTransactionException lateReportedTransactionException)
                {
                    httpContext.Response.StatusCode = (int) lateReportedTransactionException.ErrorStatusCode;
                }
                else
                {
                    httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                }
            }
        }
    }
}