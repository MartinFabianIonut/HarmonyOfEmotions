using HarmonyOfEmotions.ApiService.Data;
using HarmonyOfEmotions.ApiService.Interfaces;
using HarmonyOfEmotions.Domain.DataModels;
using Microsoft.EntityFrameworkCore;

namespace HarmonyOfEmotions.ApiService.Implementations
{
	public class UserTrackService(HarmonyOfEmotionsDbContext context, ILogger<UserTrackService> logger) : IUserTrackService
	{

		private readonly HarmonyOfEmotionsDbContext _context = context;
		private readonly ILogger<UserTrackService> _logger = logger;

		public async Task AddOrUpdateUserTrackPreference(string userId, string trackId, bool isLiked)
		{
			var existingPreference = await _context.UserTrackPreferences
				.FirstOrDefaultAsync(p => p.UserId == userId && p.TrackId == trackId);
			_logger.LogInformation("User track preference found");

			if (existingPreference == null)
			{
				var preference = new UserTrackPreference
				{
					UserId = userId,
					TrackId = trackId,
					IsLiked = isLiked
				};
				_context.UserTrackPreferences.Add(preference);
				_logger.LogInformation("User track preference added");
			}
			else
			{
				existingPreference.IsLiked = isLiked;
				_context.UserTrackPreferences.Update(existingPreference);
				_logger.LogInformation("User track preference updated");
			}

			await _context.SaveChangesAsync();
			_logger.LogInformation("User track preference saved");
		}

		public async Task<IEnumerable<UserTrackPreference>> GetUserTrackPreferences(string userId)
		{
			_logger.LogInformation("Getting user track preferences");
			return await _context.UserTrackPreferences
				.Where(p => p.UserId == userId)
				.ToListAsync();
		}

		public async Task DeleteUserTrackPreference(string userId, string trackId)
		{
			var preference = await _context.UserTrackPreferences
				.FirstOrDefaultAsync(p => p.UserId == userId && p.TrackId == trackId);
			_logger.LogInformation("User track preference found");

			if (preference != null)
			{
				_context.UserTrackPreferences.Remove(preference);
				await _context.SaveChangesAsync();
				_logger.LogInformation("User track preference deleted");
			}
		}
	}
}
