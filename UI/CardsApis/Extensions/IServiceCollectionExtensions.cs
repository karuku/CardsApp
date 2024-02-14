using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Presentation;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Domain;

namespace CardsApis.Extensions
{
	public static class IServiceCollectionExtensions
    {
        internal static IServiceCollection AddSwaggerService(this IServiceCollection services)
        {
            return services.AddSwaggerGen(c =>
            {
                #region swagger doc

                string desc = string.Format("{0} {1}",
                    " Welcome to CardsApis.", "<br/>",
                    "This is a protected API.", "<br/>");
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CardsApis",
                    Version = "v1",
                    Description = desc,
                    Contact = new OpenApiContact
                    {
                        Name = "Robert Karuku",
                        Email = "karukurobert@gmail.com",
                        Url = new Uri(@"https://www.linkedin.com/in/robert-karuku-7a2154114/"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "CardsApis LICX",
                        Url = new Uri(@"https://www.linkedin.com/in/robert-karuku-7a2154114/"),
                    }
                });

                #endregion
                  
				#region bearer security

				#region security defination

				var schemeName_bearer = "Bearer";
				var description_bearer = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                                Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n
                                Example: 'Bearer 12345abcdef'";
				description_bearer = description_bearer.Replace(@"\r\n\r\n", "<br/>");

				var securityScheme_bearer = new OpenApiSecurityScheme
				{
					Description = description_bearer,
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					Scheme = schemeName_bearer
				};

				c.AddSecurityDefinition(schemeName_bearer, securityScheme_bearer);
				#endregion

				#region securiry requirement

				var key_bearer = new OpenApiSecurityScheme
				{
					Reference = new OpenApiReference
					{
						Type = ReferenceType.SecurityScheme,
						Id = schemeName_bearer
					}
				};
				var requirement_bearer = new OpenApiSecurityRequirement {
					{
						key_bearer,
						new List<string>()
					}
				};

				c.AddSecurityRequirement(requirement_bearer);

				#endregion

				#endregion

				// Set the comments path for the Swagger JSON and UI.
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        internal static IServiceCollection AddHttpContextAccessorService(this IServiceCollection services)
        {
            return services.AddHttpContextAccessor();
        }
       
        internal static IServiceCollection AddApiServices(this IServiceCollection services,IConfiguration configuration)
        {
            return services
                .AddHttpContextAccessorService()
				.AddSwaggerService()
                .AddTransient<ICurrentUser, AuthenticatedUser>((prov) =>
				{
                    var ctx = prov.GetRequiredService<IHttpContextAccessor>();
                    var claimsPrincipal = ctx.HttpContext?.User;
                    var username = claimsPrincipal?.FindFirst(c => c.Type == ClaimTypes.Name)?.Value;
                    var uniqueUserId = claimsPrincipal?.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                    var role = claimsPrincipal?.FindFirst(c => c.Type == ClaimTypes.Role)?.Value;
                    return new AuthenticatedUser
                    {
                        Role = role,
                        UserId = uniqueUserId,
                        UserName = username
                    };
				});
        }

    }
}
