using HarmonyOfEmotions.Domain.RecommenderSystem;

namespace HarmonyOfEmotions.ApiService.Interfaces
{
	public interface IMusicBrainzService
	{
		Task<Artist?> GetArtistDetailsAsync(string artistId);
	}
}
