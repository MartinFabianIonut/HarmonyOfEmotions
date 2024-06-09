using HarmonyOfEmotions.Domain;

namespace HarmonyOfEmotions.ApiService.Interfaces
{
	public interface ISpotifyService
	{
		Task<Track[]> SearchTraksByKeyword(string keyword);
		Task<Track[]> GetTopTracksForArtist(string artistId);
	}
}
