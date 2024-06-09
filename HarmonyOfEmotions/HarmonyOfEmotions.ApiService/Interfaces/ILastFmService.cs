using HarmonyOfEmotions.Domain;

namespace HarmonyOfEmotions.ApiService.Interfaces
{
	public interface ILastFmService
	{
		Task<string?> GetArtistCorrectionAsync(string artist);
		Task<Artist?> GetArtistInfoAsync(string artist);
	}
}
