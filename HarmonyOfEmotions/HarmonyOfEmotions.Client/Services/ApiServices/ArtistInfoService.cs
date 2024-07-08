using HarmonyOfEmotions.Domain.RecommenderSystem;

namespace HarmonyOfEmotions.Client.Services.ApiServices
{
	public class ArtistInfoService(ApiClient apiClient)
	{
		private readonly ApiClient _apiClient = apiClient;

		public async Task<Artist?> GetArtistInfo(string artist)
		{
			return await _apiClient.GetAsync<Artist>($"ArtistInfo/GetArtistInfo?artist={artist}");
		}
	}
}
