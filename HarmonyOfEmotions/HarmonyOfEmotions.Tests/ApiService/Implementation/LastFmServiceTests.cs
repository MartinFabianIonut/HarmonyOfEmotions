using HarmonyOfEmotions.ApiService.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
namespace HarmonyOfEmotions.Tests.ApiService.Implementation
{
	[TestClass]
	public class LastFmServiceTests
	{
		private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
		private readonly HttpClient _httpClient;
		private readonly Mock<IConfiguration> _mockConfiguration;
		private readonly Mock<ILogger<LastFmService>> _mockLogger;
		private readonly LastFmService _service;

		public LastFmServiceTests()
		{
			_mockHttpMessageHandler = new Mock<HttpMessageHandler>();
			_httpClient = new HttpClient(_mockHttpMessageHandler.Object)
			{
				BaseAddress = new Uri("https://example.com/") // Set a valid base address
			};
			_mockConfiguration = new Mock<IConfiguration>();
			_mockLogger = new Mock<ILogger<LastFmService>>();
			_service = new LastFmService(_httpClient, _mockConfiguration.Object, _mockLogger.Object);
		}

		[TestMethod]
		public async Task GetArtistInfoAsync_ReturnsArtistInfo()
		{
			// Arrange
			var artistName = "Artist";
			_mockConfiguration.Setup(c => c["LastFM:ApiKey"]).Returns("api_key");
			var expectedResponse = "{\"artist\": {\"name\": \"Artist\"}}";
			var response = new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.OK,
				Content = new StringContent(expectedResponse)
			};
			_mockHttpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(response);

			// Act
			var result = await _service.GetArtistInfoAsync(artistName);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(artistName, result.Name);
		}

		[TestMethod]
		public async Task GetArtistInfoAsync_ReturnsNull_WhenArtistNotFound()
		{
			// Arrange
			var artistName = "Unknown Artist";
			_mockConfiguration.Setup(c => c["LastFM:ApiKey"]).Returns("api_key");
			var response = new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.NotFound
			};

			_mockHttpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(response);

			// Act
			try
			{
				await _service.GetArtistInfoAsync(artistName);
			}
			catch (Exception ex)
			{
				// Assert
				Assert.IsInstanceOfType(ex, typeof(HttpRequestException));
			}
		}

		[TestMethod]
		public async Task GetArtistCorrectionAsync_ReturnsCorrectedArtist()
		{
			// Arrange
			var artistName = "Artist";
			_mockConfiguration.Setup(c => c["LastFM:ApiKey"]).Returns("api_key");
			var expectedResponse = "{\"corrections\": {\"correction\": {\"artist\": {\"name\": \"Corrected Artist\"}}}}";
			var response = new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.OK,
				Content = new StringContent(expectedResponse)
			};

			_mockHttpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(response);

			// Act
			var result = await _service.GetArtistCorrectionAsync(artistName);

			Assert.IsNotNull(result);
			Assert.AreEqual("Corrected Artist", result);
		}

	}
}
