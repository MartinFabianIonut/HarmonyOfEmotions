using HarmonyOfEmotions.Domain.Authentication;
using Microsoft.AspNetCore.Authentication;

namespace HarmonyOfEmotions.Client.Services.Authentication
{
	public class TokenService(IHttpContextAccessor httpContextAccessor) : ITokenService
	{
		private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

		public Task<string?> GetAccessTokenAsync()
		{
			try
			{
				var accessToken = _httpContextAccessor?.HttpContext?.Session.GetString(AuthState.AccessToken.ToString());
				var tokenExpiration = _httpContextAccessor?.HttpContext?.Session.GetString(AuthState.TokenExpiration.ToString());

				if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(tokenExpiration))
				{
					return Task.FromResult<string?>(null);
				}

				if (DateTime.TryParse(tokenExpiration, out var expiration) && expiration > DateTime.UtcNow)
				{
					return Task.FromResult<string?>(accessToken);
				}

				return Task.FromResult<string?>(AuthState.TokenExpired.ToString());
			}
			catch (Exception)
			{
				return Task.FromResult<string?>(null);
			}
		}

		public Task<string?> GetRefreshTokenAsync()
		{
			return Task.FromResult(_httpContextAccessor?.HttpContext?.Session.GetString(AuthState.RefreshToken.ToString()));
		}

		public async Task SetTokens(string? accessToken, string? refreshToken, int expiresIn)
		{
			if (_httpContextAccessor.HttpContext == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
			{
				_httpContextAccessor.HttpContext.Session.Clear();
				return;
			}
			_httpContextAccessor.HttpContext.Session.SetString(AuthState.AccessToken.ToString(), accessToken ?? string.Empty);
			_httpContextAccessor.HttpContext.Session.SetString(AuthState.RefreshToken.ToString(), refreshToken ?? string.Empty);
			_httpContextAccessor.HttpContext.Session.SetString(AuthState.TokenExpiration.ToString(), DateTime.UtcNow.AddSeconds(expiresIn).ToString());
			
			await Task.CompletedTask;
		}
	}
}
