using HarmonyOfEmotions.Domain.DataModels;

namespace HarmonyOfEmotions.Client.Services
{
	public class UserTrackPreferencesService(ApiClient apiClient)
	{
		private readonly ApiClient _apiClient = apiClient;

		private class UserTrackPreferencesData 
		{
			public string? TrackId { get; set; }
			public bool IsLiked { get; set; }
		}

		public async Task AddOrUpdateUserTrackPreference(string trackId, bool isLiked)
		{
			var data = new UserTrackPreference { TrackId = trackId, IsLiked = isLiked };
			await _apiClient.PostAsync($"UserTrackPreferences/addOrUpdate", data);
		}

		public async Task<IEnumerable<UserTrackPreference>?> GetUserTrackPreferences()
		{
			return await _apiClient.GetAsync<IEnumerable<UserTrackPreference>>("UserTrackPreferences/get");
		}

		public async Task DeleteUserTrackPreference(string trackId)
		{
			await _apiClient.DeleteAsync($"UserTrackPreferences/delete?trackId={trackId}");
		}
	}
}
