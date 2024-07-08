using HarmonyOfEmotions.Domain.RecommenderSystem;

namespace HarmonyOfEmotions.ApiService.Interfaces
{
    public interface IRecommenderService
	{
		Task<Track[]> GetRecommendedTraks(float x, float y, float z, string userId);
		Task<string> GetEmotionForTrack(string previewUrl);
	}
}
