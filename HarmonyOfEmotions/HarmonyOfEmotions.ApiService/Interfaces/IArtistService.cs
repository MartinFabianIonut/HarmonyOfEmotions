using HarmonyOfEmotions.Domain.RecommenderSystem;

namespace HarmonyOfEmotions.ApiService.Interfaces
{
	public interface IArtistService
	{
		Task<Artist?> GetCompleteArtistInfoAsync(string artistName);
	}
}
