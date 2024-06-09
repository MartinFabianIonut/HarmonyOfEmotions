using HarmonyOfEmotions.Domain;
using HarmonyOfEmotions.ApiService.Interfaces;

namespace HarmonyOfEmotions.ApiService.Implementations
{
	public class ArtistService(
		ILastFmService lastFmService, 
		IMusicBrainzService musicBrainzService,
		ILogger<ArtistService> logger) : IArtistService
	{
		private readonly ILastFmService _lastFmService = lastFmService;
		private readonly IMusicBrainzService _musicBrainzService = musicBrainzService;
		private readonly ILogger<ArtistService> _logger = logger;
		public async Task<Artist?> GetCompleteArtistInfoAsync(string artistName)
		{
			var lastFmArtist = await _lastFmService.GetArtistInfoAsync(artistName);
			if (lastFmArtist == null)
			{
				return null;
			}
			var musicBrainzArtist = await _musicBrainzService.GetArtistDetailsAsync(lastFmArtist.Id!);
			if (musicBrainzArtist == null)
			{
				return null;
			}

			var artist = new Artist
			{
				Id = lastFmArtist.Id,
				Name = lastFmArtist.Name,
				ImageUrl = musicBrainzArtist.ImageUrl,
				Summary = lastFmArtist.Summary,
				Content = lastFmArtist.Content,
				Country = musicBrainzArtist.Country,
				Lifetime = musicBrainzArtist.Lifetime
			};

			_logger.LogInformation("Artist found with complete info: {artist}", artist);
			return artist;
		}
	}
}
