namespace HarmonyOfEmotions.Domain.RecommenderSystem
{
	public class Artist
	{
		public string? Id { get; set; }
		public string? Name { get; set; }
		public string? ImageUrl { get; set; }
		public string? Summary { get; set; }
		public string? Content { get; set; }
		public Country? Country { get; set; }
		public Lifetime? Lifetime { get; set; }
	}
}