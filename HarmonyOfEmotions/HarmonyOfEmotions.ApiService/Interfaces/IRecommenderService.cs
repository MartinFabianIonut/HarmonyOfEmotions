using HarmonyOfEmotions.Domain;

namespace HarmonyOfEmotions.ApiService.Interfaces
{
	public interface IRecommenderService
	{
		Task<Track[]> GetRecommendedTraks(float x, float y, float z);
		Task<string> GetEmotionForTrack(string previewUrl);
	}
}
