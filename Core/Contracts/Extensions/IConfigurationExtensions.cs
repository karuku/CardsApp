using Contracts.ConfigModels;
using Microsoft.Extensions.Configuration;
using Contracts.ConfigModels;
using System;

namespace Contracts.Extensions
{
	public static class IConfigurationExtensions
	{
		public static string _settingsSectionName = "AppSettings";
		public static string _jwtSectionName = "JWT";

		public static AppSettingConfig GetAppSettingConfiguration(this IConfiguration configuration)
		{
			var configModel = configuration.GetSection(_settingsSectionName)
				.Get<AppSettingConfig>();

			return configModel;
		}

		public static JwtSettingConfig GetJwtSettingConfiguration(this IConfiguration configuration)
		{
			var configModel = configuration.GetSection(_jwtSectionName)
				.Get<JwtSettingConfig>();

			return configModel;
		}

	}
}
