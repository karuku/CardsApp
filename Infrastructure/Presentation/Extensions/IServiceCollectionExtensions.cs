using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Domain.Repositories;
using Persistence.Repositories;
using Services.Abstractions;
using Services;
using Contracts.Extensions;
using Contracts.Enums;

namespace Presentation.Extensions
{
	public static class IServiceCollectionExtensions
	{
		internal static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration)
		{
			var jwtConfiguration = configuration.GetSection("JWT");
			var jwtConfig = configuration.GetJwtSettingConfiguration();
			
			services
				.AddTransient<IUnitOfWork, UnitOfWork>()
				.AddTransient(typeof(IRepository<>), typeof(Repository<>))
				.AddTransient<IServiceManager, ServiceManager>();

			return services;
		}
	
		public static IServiceCollection AddClientServices(this IServiceCollection services, MigrationDbTypesEnum serverType, IConfiguration configuration)
		{
			return services
				.AddClientDbContexts(serverType, configuration)
				.AddAppServices(configuration);

		}
	}
}
