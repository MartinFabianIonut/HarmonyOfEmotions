using HarmonyOfEmotions.ApiService.Implementations;
using HarmonyOfEmotions.ApiService.Interfaces;
using HarmonyOfEmotions.Domain.RecommenderSystem;
using Microsoft.Extensions.Logging;
using Moq;

namespace HarmonyOfEmotions.Tests.ApiService.Implementation
{
	[TestClass]
	public class ArtistServiceTests
	{
		private Mock<ILastFmService> _mockLastFmService;
		private Mock<IMusicBrainzService> _mockMusicBrainzService;
		private Mock<ILogger<ArtistService>> _mockLogger;
		private ArtistService _artistService;

		[TestInitialize]
		public void Setup()
		{
			_mockLastFmService = new Mock<ILastFmService>();
			_mockMusicBrainzService = new Mock<IMusicBrainzService>();
			_mockLogger = new Mock<ILogger<ArtistService>>();
			_artistService = new ArtistService(_mockLastFmService.Object, _mockMusicBrainzService.Object, _mockLogger.Object);
		}

		[TestMethod]
		public async Task GetCompleteArtistInfoAsync_ReturnsCompleteArtistInfo()
		{
			// Arrange
			var artistName = "Artist";
			var lastFmArtist = new Artist { Id = "1", Name = artistName, Summary = "summary", Content = "content" };
			var musicBrainzArtist = new Artist { ImageUrl = "imageUrl", Country = new Country { Name = "Romania", Code = "RO" }, Lifetime = new Lifetime { Begin = DateTime.Now } };

			_mockLastFmService.Setup(s => s.GetArtistInfoAsync(artistName)).ReturnsAsync(lastFmArtist);
			_mockMusicBrainzService.Setup(s => s.GetArtistDetailsAsync(lastFmArtist.Id!)).ReturnsAsync(musicBrainzArtist);

			// Act
			var result = await _artistService.GetCompleteArtistInfoAsync(artistName);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(artistName, result!.Name);
			Assert.AreEqual("summary", result.Summary);
			Assert.AreEqual("imageUrl", result.ImageUrl);
		}

		[TestMethod]
		public async Task GetCompleteArtistInfoAsync_ReturnsNull_WhenArtistNotFound()
		{
			// Arrange
			var artistName = "Unknown Artist";
			_mockLastFmService.Setup(s => s.GetArtistInfoAsync(artistName)).ReturnsAsync((Artist?)null);

			// Act
			var result = await _artistService.GetCompleteArtistInfoAsync(artistName);

			// Assert
			Assert.IsNull(result);
		}
	}
}
