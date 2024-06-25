using SpotifyAPI.Web;
using HarmonyOfEmotions.ApiService.Authentication;
using HarmonyOfEmotions.Domain;
using HarmonyOfEmotions.ApiService.Interfaces;
using HarmonyOfEmotions.ServiceDefaults.Utils;
using HarmonyOfEmotions.Domain.Exceptions;

namespace HarmonyOfEmotions.ApiService.Implementations
{
	public class SpotifyService(SpotifyClientBuilder spotifyClientBuilder, ILogger<SpotifyService> logger) : Centroid, ISpotifyService
	{
		private readonly SpotifyClientBuilder _spotifyClientBuilder = spotifyClientBuilder;
		private readonly ILogger<SpotifyService> _logger = logger;

		public async Task<Track[]> GetTopTracksForArtist(string artistId)
		{
			try
			{
				var spotifyClient = await _spotifyClientBuilder.BuildClientAsync();
				var artistTopTracksRequest = new ArtistsTopTracksRequest("DE");
				var response = await spotifyClient.Artists.GetTopTracks(artistId, artistTopTracksRequest);
				var topTracks = response.Tracks.Where(t => t.PreviewUrl != null).ToList();
				_logger.LogInformation("Top tracks found for artist {ArtistId}", artistId);

				return [.. TrackUtils.ConvertToTracks(topTracks)];
			}
			catch (Exception ex) {
				_logger.LogError(ex, "Error while getting top tracks for artist {ArtistId}", artistId);
				throw new ExternalServiceException(ServiceName.SpotifyApiService, ex);
			}
		}

		public async Task<Track[]> SearchTraksByKeyword(string keyword)
		{
			try
			{
				var spotifyClient = await _spotifyClientBuilder.BuildClientAsync();
				var searchRequest = new SearchRequest(SearchRequest.Types.Track, keyword)
				{
					Market = "DE",
					Limit = 40
				};
				var response = await spotifyClient.Search.Item(searchRequest);
				_logger.LogInformation("Tracks found for keyword {Keyword}", keyword);

				var tracksWithPreview = response.Tracks.Items!.Where(t => t.PreviewUrl != null).ToList();

				return [.. TrackUtils.ConvertToTracks(tracksWithPreview)];
			}
			catch (Exception ex) {
				_logger.LogError(ex, "Error while searching tracks by keyword {Keyword}", keyword);
				throw new ExternalServiceException(ServiceName.SpotifyApiService, ex);
			}
		}
	}
}