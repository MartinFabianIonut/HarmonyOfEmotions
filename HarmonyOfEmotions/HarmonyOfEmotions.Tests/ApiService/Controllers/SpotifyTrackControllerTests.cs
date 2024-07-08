using HarmonyOfEmotions.ApiService.Controllers;
using HarmonyOfEmotions.ApiService.Interfaces;
using HarmonyOfEmotions.Domain.RecommenderSystem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HarmonyOfEmotions.Tests.ApiService.Controllers
{
    [TestClass]
	public class SpotifyTrackControllerTests
	{
		private Mock<ISpotifyService> _mockSpotifyService;
		private SpotifyTrackController _spotifyTrackController;
		private Mock<ILogger<SpotifyTrackController>> _mockLogger;

		[TestInitialize]
		public void Setup()
		{
			_mockSpotifyService = new Mock<ISpotifyService>();
			_mockLogger = new Mock<ILogger<SpotifyTrackController>>();
			_spotifyTrackController = new SpotifyTrackController(_mockLogger.Object, _mockSpotifyService.Object);
		}

		[TestMethod]
		public async Task SearchTraksByKeyword_ReturnsTracks()
		{
			// Arrange
			var keyword = "Keyword";
			var trackInfos = new Track[]
			{
				new() { Name = "Track", Artist = "Artist" },
				new () { Name = "Track2", Artist = "Artist2" }

			};

			_mockSpotifyService.Setup(s => s.SearchTraksByKeyword(keyword)).ReturnsAsync(trackInfos);

			// Act
			var result = await _spotifyTrackController.SearchTraksByKeyword(keyword);

			var objectResult = result as ObjectResult;
			var statusCode = objectResult?.StatusCode;
			var trackInfoResult = objectResult?.Value as Track[];

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(trackInfoResult);
			Assert.AreEqual(200, statusCode);
			Assert.AreEqual(2, trackInfoResult.Length);
			Assert.AreEqual("Track", trackInfoResult[0].Name);
		}

		[TestMethod]
		public async Task GetTopTracksForArtist_ReturnsTracks()
		{
			// Arrange
			var artistId = "ArtistId";
			var trackInfos = new Track[]
			{
				new() { Name = "Track", Artist = "Artist" },
				new () { Name = "Track2", Artist = "Artist2" }

			};

			_mockSpotifyService.Setup(s => s.GetTopTracksForArtist(artistId)).ReturnsAsync(trackInfos);

			// Act
			var result = await _spotifyTrackController.GetTopTracksForArtist(artistId);

			var objectResult = result as ObjectResult;
			var statusCode = objectResult?.StatusCode;
			var trackInfoResult = objectResult?.Value as Track[];

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(trackInfoResult);
			Assert.AreEqual(200, statusCode);
			Assert.AreEqual(2, trackInfoResult.Length);
			Assert.AreEqual("Track", trackInfoResult[0].Name);
		}

		[TestMethod]
		public async Task SearchTraksByKeyword_ReturnsBadRequest_WhenKeywordIsNull()
		{
			// Arrange
			string? keyword = null;

			// Act
			var result = await _spotifyTrackController.SearchTraksByKeyword(keyword);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(BadRequestResult));
		}

		[TestMethod]
		public async Task GetTopTracksForArtist_ReturnsBadRequest_WhenArtistIdIsNull()
		{
			// Arrange
			string? artistId = null;

			// Act
			var result = await _spotifyTrackController.GetTopTracksForArtist(artistId);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(BadRequestResult));
		}
	}
}
