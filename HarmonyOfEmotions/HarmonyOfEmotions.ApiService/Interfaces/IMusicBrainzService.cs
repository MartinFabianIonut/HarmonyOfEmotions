using HarmonyOfEmotions.Domain;

namespace HarmonyOfEmotions.ApiService.Interfaces
{
	public interface IMusicBrainzService
	{
		Task<Artist?> GetArtistDetailsAsync(string artistId);
	}
}
