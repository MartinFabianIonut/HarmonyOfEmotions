using HarmonyOfEmotions.Domain.Authentication;
using System.Net.Http.Headers;

namespace HarmonyOfEmotions.Client.Services.Authentication
{
	public class BearerTokenHandler(ITokenService tokenService, IServiceProvider serviceProvider) : DelegatingHandler
	{
		private readonly ITokenService _tokenService = tokenService;
		private readonly IServiceProvider _serviceProvider = serviceProvider;

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var token = await _tokenService.GetAccessTokenAsync();
			if (token == AuthState.TokenExpired.ToString())
			{
				using var scope = _serviceProvider.CreateScope();
				var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
				if (await authService.RefreshTokenIfNeeded())
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

}
