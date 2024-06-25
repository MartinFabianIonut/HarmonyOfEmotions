using HarmonyOfEmotions.Domain;
using System.Globalization;

namespace HarmonyOfEmotions.Client.Services.ApiServices
{
	public class MusicRecommenderSystemService(ApiClient apiClient)
	{
		private readonly ApiClient _apiClient = apiClient;

		public async Task<IEnumerable<Track>?> GetRecommendedTracks(float x, float y, float z)
		{
			string xString = x.ToString("0.0000000", CultureInfo.InvariantCulture);
			string yString = y.ToString("0.0000000", CultureInfo.InvariantCulture);
			string zString = z.ToString("0.0000000", CultureInfo.InvariantCulture);

			string url = $"MusicRecommenderSystem/GetRecommendedTraks?x={xString}&y={yString}&z={zString}";
			return await _apiClient.GetAsync<IEnumerable<Track>>(url);
		}

		public async Task<string?> GetEmotionForTrack(string previewUrl)
		{
			return await _apiClient.GetAsync<string>($"MusicRecommenderSystem/GetEmotionForTrack?previewUrl={previewUrl}");
		}
	}
}
