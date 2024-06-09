using HarmonyOfEmotions.Domain;

namespace HarmonyOfEmotions.Client.Services
{
	public class MusicRecommenderSystemService(ApiClient apiClient)
	{
		private readonly ApiClient _apiClient = apiClient;

		public async Task<IEnumerable<Track>?> GetRecommendedTracks(float x, float y, float z)
		{
			return await _apiClient.GetAsync<IEnumerable<Track>>($"MusicRecommenderSystem/GetRecommendedTraks?x={x}&y={y}&z={z}");
		}

		public async Task<string?> GetEmotionForTrack(string previewUrl)
		{
			return await _apiClient.GetAsync<string>($"MusicRecommenderSystem/GetEmotionForTrack?previewUrl={previewUrl}");
		}
	}
}
