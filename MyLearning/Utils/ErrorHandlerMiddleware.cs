using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace MyLearning.Utils
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            } catch (Exception ex)
            {
                
                if ( ex.InnerException != null )
                {
                    // LogWriter.LogWrite(ex.InnerException.Message);
                    // LogWriter.LogWrite(ex.StackTrace);
                }
                // LogWriter.LogWrite(ex.Message);
                // LogWriter.LogWrite(ex.StackTrace);
                await HandleErrorAsync(context, ex);
            }
        }

        private static Task HandleErrorAsync(HttpContext context, Exception exception)
        {
            switch(exception)
            {
                case AppException e:
                    // custom application error
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case KeyNotFoundException e:
                    // not found error
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default: 
                    // unhandled error
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            var response = new { Status = false, Message = exception.Message, Data = new { } };
            var payload = JsonConvert.SerializeObject(response);
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(payload);
        }
    }
}