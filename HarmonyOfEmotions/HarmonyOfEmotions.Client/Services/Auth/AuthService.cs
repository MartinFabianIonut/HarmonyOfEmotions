using System.Net.Http.Headers;
using System.Text.Json.Serialization;

namespace HarmonyOfEmotions.Client.Services.Auth
{
	public interface IAuthService
	{
		Task<bool> Login(string email, string password);
		Task<bool> Register(string email, string password);
		Task Logout();
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
		Task<string?> GetTokenAsync();
		void SetToken(string token);
	}

	public class TokenService : ITokenService
	{
		private string? _token;

		public Task<string?> GetTokenAsync()
		{
			return Task.FromResult(_token);
		}

		public void SetToken(string token)
		{
			_token = token;
		}
	}


	public class BearerTokenHandler(ITokenService tokenService) : DelegatingHandler
	{
		private readonly ITokenService _tokenService = tokenService;

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var token = await _tokenService.GetTokenAsync();
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
			return await base.SendAsync(request, cancellationToken);
		}
	}

	public class AuthService(ApiClient httpClient, ITokenService tokenService) : IAuthService
	{
		private readonly ApiClient _httpClient = httpClient;
		private readonly ITokenService _tokenService = tokenService;

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
				_tokenService.SetToken(authResponse?.AccessToken!);
				return true;
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
			return response.IsSuccessStatusCode;
		}

		public async Task Logout()
		{
			await Task.CompletedTask;
		}
	}
}
