using HarmonyOfEmotions.ApiService.Data;
using HarmonyOfEmotions.ApiService.Interfaces;
using HarmonyOfEmotions.Domain.DataModels;
using HarmonyOfEmotions.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace HarmonyOfEmotions.ApiService.Implementations
{
	public class UserTrackService(HarmonyOfEmotionsDbContext context, ILogger<UserTrackService> logger) : IUserTrackService
	{

		private readonly HarmonyOfEmotionsDbContext _context = context;
		private readonly ILogger<UserTrackService> _logger = logger;

		public async Task AddOrUpdateUserTrackPreference(string userId, string trackId, bool isLiked)
		{
			try
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
			catch (Exception userTrackPreferencesException)
			{
				throw new InternalServerErrorException(ServiceName.FirebaseSaveService, userTrackPreferencesException);
			}
		}

		public async Task<IEnumerable<UserTrackPreference>> GetUserTrackPreferences(string userId)
		{
			try
			{
				_logger.LogInformation("Getting user track preferences");
				return await _context.UserTrackPreferences
					.Where(p => p.UserId == userId)
					.ToListAsync();
			}
			catch (Exception userTrackPreferencesException)
			{
				throw new InternalServerErrorException(ServiceName.FirebaseRepositoryService, userTrackPreferencesException);
			}
		}

		public async Task DeleteUserTrackPreference(string userId, string trackId)
		{
			try
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
			catch (Exception userTrackPreferencesException)
			{
				throw new InternalServerErrorException(ServiceName.FirebaseSaveService, userTrackPreferencesException);
			}
		}
	}
}
