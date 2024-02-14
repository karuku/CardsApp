using Contracts.Enums;
using Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Services.Exceptions;
using System;

namespace Presentation.Extensions
{
	public static class DbContextExtensions
	{
		internal static string GetDbConnectionString(this IConfiguration configuration,string connStrName="Database")
		{
			return configuration.GetConnectionString(connStrName);
		}
		internal static IServiceCollection AddApplicationDbContextTemplate<TContext>(this
			IServiceCollection services, string connectionString,
			MigrationDbTypesEnum serverType, ServiceLifetime lifetime = ServiceLifetime.Scoped,
			bool isPooled=false)
			where TContext : DbContext
		{
			
			Action<DbContextOptionsBuilder> options = (builder) =>
			{
				switch (serverType)
				{
					case MigrationDbTypesEnum.PostGres:
						builder.UseNpgsql(connectionString);

						break;
					case MigrationDbTypesEnum.MySQL:
						builder.UseMySQL(connectionString);
						break;
					case MigrationDbTypesEnum.SQLite:
						builder.UseSqlite(connectionString);
						break;
					case MigrationDbTypesEnum.SQLServer:
						builder.UseSqlServer(connectionString);
						break;
					case MigrationDbTypesEnum.None:
					default:
						builder.UseInMemoryDatabase(connectionString);
						break;
				}
			};
			// Configure DbContext with Scoped lifetime
			if(isPooled)
				services.AddDbContextPool<TContext>(builder =>
				{
					options(builder);
				});
			else
				services.AddDbContext<TContext>(builder =>
				{
					options(builder);
				}, lifetime);

			return services;
		}

		internal static IServiceCollection AddAppDbContext(this IServiceCollection services, IConfiguration configuration, MigrationDbTypesEnum serverType, ServiceLifetime lifetime = ServiceLifetime.Scoped)
		{
			var connectionString = configuration.GetDbConnectionString();

			// Configure DbContext with provided Scoped lifetime    
			services
				.AddApplicationDbContextTemplate<ApplicationDbContext>(connectionString,
				serverType, lifetime);

			services.AddDefaultIdentity<ApplicationUser>(options =>
			options.SignIn.RequireConfirmedAccount = true)
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>();

			return services;
		}
		
		internal static IServiceCollection AddClientDbContexts(this IServiceCollection services, MigrationDbTypesEnum serverType, IConfiguration configuration)
		{
			return services
				.AddAppDbContext(configuration, serverType);
		}

	}
}
