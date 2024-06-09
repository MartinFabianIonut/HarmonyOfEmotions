using HarmonyOfEmotions.Client.Components.Pages;
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

		// private cached user track preferences
		private IEnumerable<UserTrackPreference>? _userTrackPreferences;

		public async Task<bool> IsTrackLiked(string trackId)
		{
			if (_userTrackPreferences == null)
			{
				await GetUserTrackPreferences();
			}
			return _userTrackPreferences?.FirstOrDefault(x => x.TrackId == trackId)?.IsLiked ?? false;
		}

		public async Task<bool> IsTrackDisliked(string trackId)
		{
			if (_userTrackPreferences == null)
			{
				await GetUserTrackPreferences();
			}
			return _userTrackPreferences?.FirstOrDefault(x => x.TrackId == trackId)?.IsLiked == false;
		}

		public async Task AddOrUpdateUserTrackPreference(string trackId, bool isLiked)
		{
			var data = new UserTrackPreference { TrackId = trackId, IsLiked = isLiked };
			await _apiClient.PostAsync($"UserTrackPreferences/addOrUpdate", data);
			await GetUserTrackPreferences();
		}

		public async Task<IEnumerable<UserTrackPreference>?> GetUserTrackPreferences()
		{
			_userTrackPreferences = await _apiClient.GetAsync<IEnumerable<UserTrackPreference>>("UserTrackPreferences/get");
			return _userTrackPreferences;
		}

		public async Task DeleteUserTrackPreference(string trackId)
		{
			await _apiClient.DeleteAsync($"UserTrackPreferences/delete?trackId={trackId}");
			await GetUserTrackPreferences();
		}
	}
}
