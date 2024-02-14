using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Presentation.Helpers;
using Services.Abstractions;
using Domain.Enums;
using Persistence.Exceptions;

namespace Presentation.Extensions
{
	public static class IApplicationBuilderExtensions
	{
		public static async Task InitDatabase(this IApplicationBuilder app)
		{
			try
			{
				using (var scope = app.ApplicationServices.CreateScope())
				{
					var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
					await dbContext.Database.MigrateAsync();
				}
				await app.UpdateRoles();
				await app.UpdateDefaultUsers();
			}
			catch (InvalidOperationException ex)
			{
				throw new InvalidDatabaseServerException(ex.Message);
			}
			catch (Exception ex)
			{

				throw;
			}
		}
		public static async Task UpdateDefaultUsers(this IApplicationBuilder app)
		{
			using (var scope = app.ApplicationServices.CreateScope())
			{
				var serviceManager = scope.ServiceProvider.GetRequiredService<IServiceManager>();
				var userManager = serviceManager.AuthService.UserManager;
				var res = await userManager.Users.ToListAsync();

				if (res == null || res.Count <= 0)
				{
					var defaultUsers = AppDefaults.DefaultUsers;
					foreach(var user in defaultUsers)
					{
						var userRes = await userManager.CreateAsync(user, AppDefaults.DefaultUserPassword);
						if(userRes.Succeeded)
							if (user.UserName.ToLower() == "admin@logicea.com")
								await userManager.AddToRoleAsync(user, SysRoleTypes.Admin.ToString());
							else
								await userManager.AddToRoleAsync(user, SysRoleTypes.Member.ToString());
					}
					
				}

			}

		}
		public static async Task UpdateRoles(this IApplicationBuilder app)
		{
			using (var scope = app.ApplicationServices.CreateScope())
			{
				var serviceManager = scope.ServiceProvider.GetRequiredService<IServiceManager>();
				var roleManager = serviceManager.AuthService.RoleManager;
				var res = await roleManager.Roles.ToListAsync();
				if (res == null || res.Count<=0)
				{
					var sysRoles = Enum.GetValues<SysRoleTypes>().ToList();
					foreach(var role in sysRoles)
					{
						var roleRes = await roleManager.CreateAsync(new IdentityRole(role.ToString()));
					}
					
				}
			}

		}
		
	}
}
