using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Domain.Exceptions;
using Newtonsoft.Json;
using Contracts.ResModels;

namespace CardsApis.Extensions
{
	public static class HttpContextExtensions
    {
        public static IServiceProvider GetRequestServices(this HttpContext httpContext)
        {
            try
            {
                var provider =
                     httpContext.RequestServices;

                return provider;
            }
            catch (Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        public static T GetService<T>(this HttpContext httpContext)
            where T : class
        {
            try
            {
                var apiDIService =
                     httpContext.RequestServices.GetService<T>();

                return apiDIService;
            }
            catch (Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        public static T GetRequiredService<T>(this HttpContext httpContext)
            where T : class
        {
            try
            {
                var apiDIService =
                     httpContext.RequestServices.GetRequiredService<T>();

                return apiDIService;
            }
            catch (Exception ex)
            {
                throw ex.GetBaseException();
            }
        }
         
        public static async Task HandleExceptionAsync(this HttpContext httpContext, Exception exception)
        {
            httpContext.Response.ContentType = "application/json";
            switch (exception)
            {
                case BadRequestException _:
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    break;
                case UnAuthorizedException _:
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    break;
                default:
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    break;
            }

            var errorRes = new ApiErrorRes
            {
                StatusCode = (int)httpContext.Response.StatusCode,
                Message = exception.Message
            };
            await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(errorRes));
        }

    }
}
