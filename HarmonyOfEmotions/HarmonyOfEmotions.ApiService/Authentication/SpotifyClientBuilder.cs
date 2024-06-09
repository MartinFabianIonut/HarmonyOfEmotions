using SpotifyAPI.Web;

namespace HarmonyOfEmotions.ApiService.Authentication
{
	public class SpotifyClientBuilder(SpotifyClientConfig spotifyClientConfig, IConfiguration configuration)
	{
		private readonly IConfiguration _configuration = configuration;
		private readonly SpotifyClientConfig _spotifyClientConfig = spotifyClientConfig;

		public async Task<SpotifyClient> BuildClientAsync()
		{
			var request = new ClientCredentialsRequest(_configuration["Spotify:ClientId"]!, _configuration["Spotify:ClientSecret"]!);
			var response = await new OAuthClient(_spotifyClientConfig).RequestToken(request);

			var spotify = new SpotifyClient(_spotifyClientConfig.WithToken(response.AccessToken));

			return spotify;
		}
	}
}
