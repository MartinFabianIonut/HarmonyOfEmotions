using HarmonyOfEmotions.Domain.Authentication;

namespace HarmonyOfEmotions.Client.Services.Authentication
{
	public interface IAuthService
	{
		Task<AuthResponse?> Login(string email, string password);
		Task<AuthResponse?> Register(string email, string password);
		Task Logout();
		Task<bool> RefreshToken();
		Task<bool> RefreshTokenIfNeeded();
	}
}
