using Newtonsoft.Json;

namespace HarmonyOfEmotions.Domain.Exceptions
{
	public class ErrorResponse
	{
		[JsonProperty("statusCode")]
		public int StatusCode { get; set; }

		[JsonProperty("message")]
		public string? Message { get; set; }
	}
}