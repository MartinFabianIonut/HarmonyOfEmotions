using HarmonyOfEmotions.ApiService.Controllers;
using HarmonyOfEmotions.Domain;
using HarmonyOfEmotions.ApiService.Interfaces;
using System.Text.Json;
using HarmonyOfEmotions.Domain.Exceptions;

namespace HarmonyOfEmotions.ApiService.Implementations
{
	public class LastFmService(
		HttpClient httpClient,
		IConfiguration configuration,
		ILogger<LastFmService> logger) : ILastFmService
	{
		private readonly HttpClient _httpClient = httpClient;
		private readonly string _apiKey = configuration["LastFM:ApiKey"]!;
		private readonly ILogger<LastFmService> _logger = logger;

		public async Task<string?> GetArtistCorrectionAsync(string artistName)
		{
			var response = await _httpClient.GetAsync($"?method=artist.getcorrection&artist={Uri.EscapeDataString(artistName)}&api_key={_apiKey}&format=json");
			response.EnsureSuccessStatusCode();

			var jsonResponse = await response.Content.ReadAsStringAsync();

			try
			{
				using var document = JsonDocument.Parse(jsonResponse);
				var root = document.RootElement;

				if (root.TryGetProperty("corrections", out var corrections) &&
					corrections.TryGetProperty("correction", out var correction) &&
					correction.TryGetProperty("artist", out var artist) &&
					artist.TryGetProperty("name", out var name))
				{
					_logger.LogInformation("Corrected artist name from {ArtistName} to {CorrectedArtist}", artistName, name.GetString());
					return name.GetString();
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to parse LastFM response for artist correction");
				throw new InternalServerErrorException(ServiceName.LastFMApiService, ex);
			}
		}

		public async Task<Artist?> GetArtistInfoAsync(string artistName)
		{
			var correctedArtist = await GetArtistCorrectionAsync(artistName);
			if (correctedArtist != null)
			{
				artistName = correctedArtist;
			}
			else
			{
				_logger.LogInformation("No correction found for artist name {ArtistName}", artistName);
			}
			try
			{
				// remove trailing whitespaces
				artistName = artistName.Trim();
				var response = await _httpClient.GetAsync($"?method=artist.getinfo&artist={Uri.EscapeDataString(artistName)}&api_key={_apiKey}&format=json");
				response.EnsureSuccessStatusCode();

				var jsonResponse = await response.Content.ReadAsStringAsync();


				using var document = JsonDocument.Parse(jsonResponse);
				var root = document.RootElement;

				var artist = new Artist();

				if (root.TryGetProperty("artist", out var artistJson))
				{
					if (artistJson.TryGetProperty("mbid", out var mbid))
					{
						artist.Id = mbid.GetString();
					}
					else
					{
						_logger.LogInformation("MBID not found for artist {ArtistName}", artistName);
					}
					if (artistJson.TryGetProperty("name", out var name))
					{
						artist.Name = name.GetString();
					}
					else
					{
						_logger.LogInformation("Name not found for artist {ArtistName}", artistName);
					}
					if (artistJson.TryGetProperty("bio", out var bio) &&
						bio.TryGetProperty("summary", out var summary) &&
						bio.TryGetProperty("content", out var content))
					{
						artist.Summary = summary.GetString();
						artist.Content = content.GetString();
					}
					else
					{
						_logger.LogInformation("Bio not found for artist {ArtistName}", artistName);
					}
				}
				else
				{
					_logger.LogInformation("Artist info not found for artist {ArtistName}", artistName);
				}
				return artist;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to parse LastFM response for artist info");
				return null;
			}
		}
	}
}
