using HarmonyOfEmotions.Domain;

namespace HarmonyOfEmotions.ApiService.Interfaces
{
	public interface IArtistService
	{
		Task<Artist?> GetCompleteArtistInfoAsync(string artistName);
	}
}
