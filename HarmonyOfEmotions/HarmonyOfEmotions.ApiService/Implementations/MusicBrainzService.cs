using HarmonyOfEmotions.ApiService.Interfaces;
using HarmonyOfEmotions.Domain.Exceptions;
using HarmonyOfEmotions.Domain.RecommenderSystem;
using HarmonyOfEmotions.ServiceDefaults.Exceptions;
using HarmonyOfEmotions.ServiceDefaults.Utils;
using System.Text.Json;

namespace HarmonyOfEmotions.ApiService.Implementations
{
	public class MusicBrainzService(HttpClient httpClient, ILogger<MusicBrainzService> logger) : IMusicBrainzService
	{
		private readonly HttpClient _httpClient = httpClient;
		private readonly ILogger<MusicBrainzService> _logger = logger;
		public async Task<Artist?> GetArtistDetailsAsync(string artistId)
		{
			if (string.IsNullOrWhiteSpace(artistId))
			{
				_logger.LogInformation("ArtistId is null or empty");
				return null;
			}
			var response = await _httpClient.GetAsync($"artist/{Uri.EscapeDataString(artistId)}?inc=url-rels&fmt=json");
			response.EnsureSuccessStatusCode();
			var jsonResponse = await response.Content.ReadAsStringAsync();

			try
			{
				using var document = JsonDocument.Parse(jsonResponse);
				var root = document.RootElement;

				var artist = new Artist();

				if (root.TryGetProperty("country", out var country))
				{
					artist.Country = CountryUtils.GetCountryForCode(country.GetString()!);
				}
				else
				{
					_logger.LogInformation("Country not found for artist {ArtistId}", artistId);
				}
				if (root.TryGetProperty("life-span", out var lifeSpan) &&
					lifeSpan.TryGetProperty("begin", out var begin) &&
					lifeSpan.TryGetProperty("ended", out var ended) &&
					lifeSpan.TryGetProperty("end", out var end))
				{
					artist.Lifetime = new Lifetime
					{
						Begin = DateUtils.ConvertStringToDateTime(begin.GetString()!),
						End = DateUtils.ConvertStringToDateTime(end.GetString()!),
						IsAlive = !ended.GetBoolean()
					};
				}
				else
				{
					_logger.LogInformation("Lifetime not found for artist {ArtistId}", artistId);
				}
				if (root.TryGetProperty("relations", out var relations))
				{
					foreach (var relation in relations.EnumerateArray())
					{
						if (relation.TryGetProperty("type", out var type) &&
							type.GetString() == "image" &&
							relation.TryGetProperty("url", out var url) &&
							url.TryGetProperty("resource", out var image))
						{
							var image_url = image.ToString();
							if (image_url.StartsWith("https://commons.wikimedia.org/wiki/File:"))
							{
								var filename = image_url[(image_url.LastIndexOf('/') + 1)..];
								image_url = "https://commons.wikimedia.org/wiki/Special:Redirect/file/" + filename;
							}
							artist.ImageUrl = image_url;
							break;
						}
					}
				}
				else
				{
					_logger.LogInformation("Image not found for artist {ArtistId}", artistId);
				}
				return artist;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to parse MusicBrainz response for artist details");
				throw new InternalServerErrorException(ServiceName.MusicBrainzApiService, ex);
			}
		}
	}
}
