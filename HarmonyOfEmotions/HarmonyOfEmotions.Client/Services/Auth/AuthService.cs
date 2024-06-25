using HarmonyOfEmotions.Client.Services.ApiServices;
using HarmonyOfEmotions.Client.Services.ErrorHandling;
using HarmonyOfEmotions.Domain.Exceptions;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;

namespace HarmonyOfEmotions.Client.Services.Auth
{
	public interface IAuthService
	{
		Task<bool> Login(string email, string password);
		Task<bool> Register(string email, string password);
		Task Logout();
		Task<bool> RefreshToken();
	}

	public class AuthResponse
	{
		public string? TokenType { get; set; }
		public string? AccessToken { get; set; }
		public string? RefreshToken { get; set; }
		public int ExpiresIn { get; set; }
	}

	[JsonSerializable(typeof(LoginRequest))]
	public class LoginRequest
	{
		public string? Email { get; set; }
		public string? Password { get; set; }
	}

	public interface ITokenService
	{
		Task<string?> GetAccessTokenAsync();
		Task<string?> GetRefreshTokenAsync();
		void SetTokens(string? accessToken, string? refreshToken, int expiresIn);
	}

	public class TokenService : ITokenService
	{
		private string? _accessToken;
		private string? _refreshToken;
		private DateTime? _tokenExpiration;

		public Task<string?> GetAccessTokenAsync()
		{
			if (_accessToken == null)
			{
				return Task.FromResult<string?>(null);
			}
			if (_tokenExpiration > DateTime.UtcNow)
			{
				return Task.FromResult(_accessToken)!;
			}
			return Task.FromResult<string?>("Expired");
		}

		public Task<string?> GetRefreshTokenAsync()
		{
			return Task.FromResult(_refreshToken);
		}

		public void SetTokens(string? accessToken, string? refreshToken, int expiresIn)
		{
			_accessToken = accessToken;
			_refreshToken = refreshToken;
			_tokenExpiration = DateTime.UtcNow.AddSeconds(expiresIn);
		}
	}


	public class BearerTokenHandler(ITokenService tokenService, IServiceProvider serviceProvider) : DelegatingHandler
	{
		private readonly ITokenService _tokenService = tokenService;
		private readonly IServiceProvider _serviceProvider = serviceProvider;

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var token = await _tokenService.GetAccessTokenAsync();
			if (token == "Expired")
			{
				using var scope = _serviceProvider.CreateScope();
				var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
				if (await authService.RefreshToken())
				{
					token = await _tokenService.GetAccessTokenAsync();
				}
				else
				{
					throw new Exception("Failed to refresh token");
				}
			}
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
			return await base.SendAsync(request, cancellationToken);
		}
	}

	public class AuthService(ApiClient httpClient, ITokenService tokenService, IErrorHandlingService errorHandlingService) : IAuthService
	{
		private readonly ApiClient _httpClient = httpClient;
		private readonly ITokenService _tokenService = tokenService;
		private readonly IErrorHandlingService _errorHandlingService = errorHandlingService;

		public async Task<bool> Login(string email, string password)
		{
			var loginData = new LoginRequest
			{
				Email = email,
				Password = password
			};
			var response = await _httpClient.PostAsync("login", loginData);
			if (response.IsSuccessStatusCode)
			{
				var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
				_tokenService.SetTokens(authResponse?.AccessToken!, authResponse?.RefreshToken!, authResponse.ExpiresIn!);
				return true;
			}
			else
			{
				_errorHandlingService.HandleError(new Exception("An error occurred while processing the login step."));
			}
			return false;
		}

		public async Task<bool> Register(string email, string password)
		{
			var registerData = new LoginRequest
			{
				Email = email,
				Password = password
			};
			var response = await _httpClient.PostAsync("register", registerData);
			if (!response.IsSuccessStatusCode)
			{
				_errorHandlingService.HandleError(new Exception("An error occurred while processing the registration step"));
			}
			return response.IsSuccessStatusCode;
		}

		public async Task<bool> RefreshToken()
		{
			var refreshToken = await _tokenService.GetRefreshTokenAsync();
			_tokenService.SetTokens(null, null, 100); // Clear tokens to prevent infinite loop
			var response = await _httpClient.PostAsync("refresh", new { RefreshToken = refreshToken });
			if (response.IsSuccessStatusCode)
			{
				var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
				_tokenService.SetTokens(authResponse?.AccessToken!, authResponse?.RefreshToken!, authResponse.ExpiresIn!);
				return true;
			}
			else
			{
				_errorHandlingService.HandleError(new Exception("An error occurred while processing the refresh token step."));
			}
			return false;
		}

		public async Task Logout()
		{
			_tokenService.SetTokens(null, null, 0);
			await Task.CompletedTask;
		}
	}
}
