using HarmonyOfEmotions.Domain;

namespace HarmonyOfEmotions.Client.Services
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
