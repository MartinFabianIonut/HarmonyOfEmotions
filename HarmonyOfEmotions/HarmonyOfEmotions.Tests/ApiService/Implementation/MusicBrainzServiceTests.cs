using HarmonyOfEmotions.ApiService.Implementations;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
namespace HarmonyOfEmotions.Tests.ApiService.Implementation
{
	[TestClass]
	public class MusicBrainzServiceTests
	{
		private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
		private readonly HttpClient _httpClient;
		private readonly Mock<ILogger<MusicBrainzService>> _mockLogger;
		private readonly MusicBrainzService _service;

		public MusicBrainzServiceTests()
		{
			_mockHttpMessageHandler = new Mock<HttpMessageHandler>();
			_httpClient = new HttpClient(_mockHttpMessageHandler.Object)
			{
				BaseAddress = new Uri("https://example.com/") // Set a valid base address
			};
			_mockLogger = new Mock<ILogger<MusicBrainzService>>();
			_service = new MusicBrainzService(_httpClient, _mockLogger.Object);
		}

		[TestMethod]
		public async Task GetArtistDetailsAsync_ReturnsArtistDetails()
		{
			// Arrange
			var artistId = "ArtistId";
			var expectedResponse = "{\"country\": \"US\"}";
			var response = new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.OK,
				Content = new StringContent(expectedResponse)
			};
			_mockHttpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(response);

			// Act
			var result = await _service.GetArtistDetailsAsync(artistId);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual("US", result.Country.Code);
		}

		[TestMethod]
		public async Task GetArtistDetailsAsync_ReturnsNull_WhenArtistNameIsEmpty()
		{
			// Arrange
			var artistId = string.Empty;

			// Act
			var result = await _service.GetArtistDetailsAsync(artistId);

			// Assert
			Assert.IsNull(result);
		}

	}
}
