using Newtonsoft.Json;

namespace HarmonyOfEmotions.Domain.External
{
	public class QuoteResponse
	{
		[JsonProperty("q")]
		public string? Quote { get; set; }

		[JsonProperty("a")]
		public string? Author { get; set; }
	}
}
