using Contracts.ConfigModels;
using Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.ConfigModels
{
	public class AppSettingConfig
	{
		public MigrationDbTypesEnum DbServerType { get; set; }
		public bool? MigrateDb { get; set; }
	}
}
