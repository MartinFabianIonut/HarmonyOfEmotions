using HarmonyOfEmotions.Client.Services.ApiServices;
using HarmonyOfEmotions.Domain.Authentication;

namespace HarmonyOfEmotions.Client.Services.Authentication
{
	public class AuthService(ApiClient httpClient, ITokenService tokenService) : IAuthService
	{
		private readonly ApiClient _httpClient = httpClient;
		private readonly ITokenService _tokenService = tokenService;

		public async Task<AuthResponse?> Login(string email, string password)
		{
			var loginData = new LoginRequest
			{
				Email = email,
				Password = password
			};

			var response = await _httpClient.PostAsync("login", loginData);

			var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
			if (response.IsSuccessStatusCode)
			{
				await _tokenService.SetTokens(authResponse?.AccessToken!, authResponse?.RefreshToken!, authResponse.ExpiresIn!);
			}

			return authResponse;
		}

		public async Task<AuthResponse?> Register(string email, string password)
		{
			var registerData = new LoginRequest
			{
				Email = email,
				Password = password
			};

			var response = await _httpClient.PostAsync("register", registerData);

			var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
			if (response.IsSuccessStatusCode)
			{
				await _tokenService.SetTokens(authResponse?.AccessToken!, authResponse?.RefreshToken!, authResponse.ExpiresIn!);
			}

			return authResponse;
		}

		public async Task<bool> RefreshTokenIfNeeded()
		{
			var token = await _tokenService.GetAccessTokenAsync();
			if (token == AuthState.TokenExpired.ToString())
			{
				return await RefreshToken();
			}
			else if (token == null)
			{
				return false;
			}

			return true;
		}

		public async Task<bool> RefreshToken()
		{
			var refreshToken = await _tokenService.GetRefreshTokenAsync();
			var response = await _httpClient.PostAsync("refresh", new { RefreshToken = refreshToken });
			if (response.IsSuccessStatusCode)
			{
				var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
				await _tokenService.SetTokens(authResponse?.AccessToken!, authResponse?.RefreshToken!, authResponse.ExpiresIn!);
				return true;
			}

			return false;
		}

		public Task Logout()
		{
			_tokenService.SetTokens(null, null, 0);
			return Task.CompletedTask;
		}
	}
}
