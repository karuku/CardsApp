using Domain;
using Services.Abstractions;

namespace Services.Abstractions
{
	public interface IServiceManager
	{
		ICurrentUser CurrentUser { get; }
		IAuthService AuthService { get; }
		ICardService CardService { get; }
	}
	
}
