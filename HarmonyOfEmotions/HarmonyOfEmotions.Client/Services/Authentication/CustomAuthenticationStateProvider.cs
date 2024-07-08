using HarmonyOfEmotions.Domain.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace HarmonyOfEmotions.Client.Services.Authentication
{
	public class CustomAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor, IAuthService authService) : AuthenticationStateProvider
	{
		private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
		private readonly IAuthService _authService = authService;

		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			var user = _httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal(new ClaimsIdentity());

			// If user is authenticated via cookies, refresh token if necessary
			if (user.Identity?.IsAuthenticated == true)
			{
				// Optionally refresh token if close to expiration
				var authenticated = await _authService.RefreshTokenIfNeeded();
				if (!authenticated)
				{
					await NotifyUserLogout();
					var claims = new List<Claim>
					{
						new(ClaimTypes.Expired, AuthState.SessionExpired.ToString())
					};
					var identity = new ClaimsIdentity(claims);
					user = new ClaimsPrincipal(identity);
				}
			}
			return await Task.FromResult(new AuthenticationState(user));
		}

		public async Task NotifyUserAuthentication(string email)
		{
			var claims = new List<Claim>
			{
				new(ClaimTypes.Name, email)
			};

			var identity = new ClaimsIdentity(claims, AuthState.HarmonyAuthentication.ToString());
			var user = new ClaimsPrincipal(identity);

			await _httpContextAccessor.HttpContext?.SignInAsync(AuthState.HarmonyAuthentication.ToString(), user)!;

			NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
		}

		public async Task NotifyUserLogout()
		{
			await _httpContextAccessor.HttpContext?.SignOutAsync(AuthState.HarmonyAuthentication.ToString())!;

			NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
		}
	}
}
