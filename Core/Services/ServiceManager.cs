using Contracts.ConfigModels;
using Domain;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Services.Abstractions;
using Services.Services;
using System;

namespace Services
{
	public sealed class ServiceManager : IServiceManager
	{
		private readonly Lazy<IAuthService> _lazyAuthService;
		private readonly Lazy<ICardService> _lazyCardService;
		private readonly ICurrentUser _currentUser;
		public ServiceManager(IUnitOfWork unitOfWork, ICurrentUser currentUser,
			UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, 
			IConfiguration configuration)
		{
			_currentUser = currentUser;
			_lazyAuthService = new Lazy<IAuthService>(() => new AuthService(unitOfWork,
				currentUser, userManager, roleManager,configuration));
			_lazyCardService = new Lazy<ICardService>(() => new CardService(unitOfWork, currentUser));
		}

		public ICurrentUser CurrentUser => _currentUser;
		public IAuthService AuthService => _lazyAuthService.Value;
		public ICardService CardService => _lazyCardService.Value;

	}
}
