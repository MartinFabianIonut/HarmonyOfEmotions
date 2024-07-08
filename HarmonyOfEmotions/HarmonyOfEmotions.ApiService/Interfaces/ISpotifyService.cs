using HarmonyOfEmotions.Domain.RecommenderSystem;

namespace HarmonyOfEmotions.ApiService.Interfaces
{
    public interface ISpotifyService
	{
		Task<Track[]> SearchTraksByKeyword(string keyword);
		Task<Track[]> GetTopTracksForArtist(string artistId);
	}
}
