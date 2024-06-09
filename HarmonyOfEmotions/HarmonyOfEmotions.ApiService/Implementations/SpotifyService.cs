using SpotifyAPI.Web;
using HarmonyOfEmotions.ApiService.Authentication;
using HarmonyOfEmotions.Domain;
using HarmonyOfEmotions.ApiService.Interfaces;
using HarmonyOfEmotions.ServiceDefaults.Utils;

namespace HarmonyOfEmotions.ApiService.Implementations
{
	public class SpotifyService(SpotifyClientBuilder spotifyClientBuilder, ILogger<SpotifyService> logger) : Centroid, ISpotifyService
	{
		private readonly SpotifyClientBuilder _spotifyClientBuilder = spotifyClientBuilder;
		private readonly ILogger<SpotifyService> _logger = logger;

		public async Task<Track[]> GetTopTracksForArtist(string artistId)
		{
			var spotifyClient = await _spotifyClientBuilder.BuildClientAsync();
			var artistTopTracksRequest = new ArtistsTopTracksRequest("DE");
			var response = await spotifyClient.Artists.GetTopTracks(artistId, artistTopTracksRequest);
			var topTracks = response.Tracks;
			_logger.LogInformation("Top tracks found for artist {ArtistId}", artistId);

			return [.. TrackUtils.ConvertToTracks(topTracks)];
		}

		public async Task<Track[]> SearchTraksByKeyword(string keyword)
		{
			var spotifyClient = await _spotifyClientBuilder.BuildClientAsync();
			var searchRequest = new SearchRequest(SearchRequest.Types.Track, keyword);
			var response = await spotifyClient.Search.Item(searchRequest);
			_logger.LogInformation("Tracks found for keyword {Keyword}", keyword);

			return [.. TrackUtils.ConvertToTracks(response.Tracks.Items!)];
		}
	}
}