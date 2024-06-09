using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace HarmonyOfEmotions.Client.Services.Auth
{
	public class CustomAuthenticationStateProvider : AuthenticationStateProvider
	{
		private ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

		public override Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			return Task.FromResult(new AuthenticationState(_anonymous));
		}

		public void NotifyUserAuthentication(string email)
		{
			var claims = new List<Claim>
			{
				new(ClaimTypes.Email, email),
				new(ClaimTypes.Name, email),
				new(ClaimTypes.NameIdentifier, email)
			};

			var identity = new ClaimsIdentity(claims, "apiauth_type");
			var user = new ClaimsPrincipal(identity);

			_anonymous = user;

			NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
		}

		public void NotifyUserLogout()
		{
			var anonymous = new ClaimsPrincipal(new ClaimsIdentity());

			_anonymous = anonymous;
			NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
		}
	}
}
