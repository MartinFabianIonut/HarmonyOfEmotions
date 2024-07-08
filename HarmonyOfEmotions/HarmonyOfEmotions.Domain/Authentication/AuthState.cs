using NetEscapades.EnumGenerators;
using System.ComponentModel;

namespace HarmonyOfEmotions.Domain.Authentication
{
	[EnumExtensions]
	public enum AuthState
	{
		[Description("Authenticated")]
		Authenticated,

		[Description("You are not logged in yet. Please log in to access the application.")]
		Unauthenticated,

		[Description("Your session has expired. Please log in again.")]
		SessionExpired,

		[Description("This is the cookie that will be used to authenticate the user.")]
		HarmonyAuthentication,

		AccessToken,

		RefreshToken,

		TokenExpiration,

		TokenExpired
	}
}
