using HarmonyOfEmotions.Domain.RecommenderSystem;

namespace HarmonyOfEmotions.ApiService.Interfaces
{
    public interface IUserTrackService
	{
		Task AddOrUpdateUserTrackPreference(string userId, string trackId, bool isLiked);
		Task<IEnumerable<UserTrackPreference>> GetUserTrackPreferences(string userId);
		Task DeleteUserTrackPreference(string userId, string trackId);
	}
}
