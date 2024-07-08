using System.Text.Json.Serialization;

namespace HarmonyOfEmotions.Domain.Authentication
{
	[JsonSerializable(typeof(LoginRequest))]
	public class LoginRequest
	{
		public string? Email { get; set; }
		public string? Password { get; set; }
	}
}
