using System;
using System.Threading.Tasks;
using CardsApis.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CardsApis.Middlewares
{
	public class ExceptionHandlingMiddleware 
	{
		private readonly RequestDelegate _next;

		public ExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

		public async Task InvokeAsync(HttpContext context, ILogger<ExceptionHandlingMiddleware> logger)
		{
			try
			{
				await _next(context);
			}
			catch (Exception e)
			{
				logger.LogError(e, e.Message);

				await context.HandleExceptionAsync(e);
			}
		}
	}

	// Extension method used to add the middleware to the HTTP request pipeline.
	public static class ExceptionHandlingMiddlewareExtensions
	{
		public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<ExceptionHandlingMiddleware>();
		}
	}
}
