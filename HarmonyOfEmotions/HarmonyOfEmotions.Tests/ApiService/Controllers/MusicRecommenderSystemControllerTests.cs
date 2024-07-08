using HarmonyOfEmotions.ApiService.Controllers;
using HarmonyOfEmotions.ApiService.Interfaces;
using HarmonyOfEmotions.Domain.RecommenderSystem;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HarmonyOfEmotions.Tests.ApiService.Controllers
{
    [TestClass]
	public class MusicRecommenderSystemControllerTests
	{
		private Mock<IRecommenderService> _mockMusicRecommenderSystemService;
		private MusicRecommenderSystemController _musicRecommenderSystemController;
		private Mock<ILogger<MusicRecommenderSystemController>> _mockLogger;

		[TestInitialize]
		public void Setup()
		{
			_mockMusicRecommenderSystemService = new Mock<IRecommenderService>();
			_mockLogger = new Mock<ILogger<MusicRecommenderSystemController>>();
			_musicRecommenderSystemController = new MusicRecommenderSystemController(_mockLogger.Object, _mockMusicRecommenderSystemService.Object);

			var userId = "userId";
			var mockUser = new ClaimsPrincipal(new ClaimsIdentity(
			[
				new Claim(ClaimTypes.NameIdentifier, userId)
			], "mock"));

			_musicRecommenderSystemController.ControllerContext = new ControllerContext()
			{
				HttpContext = new DefaultHttpContext() { User = mockUser }
			};
		}

		[TestMethod]
		public async Task GetRecommendedTraks_ReturnsRecommendations()
		{
			// Arrange
			var x = 1.0f;
			var y = 2.0f;
			var z = 3.0f;
			var userId = "userId";
			var recommendedTracks = new Track[]
			{
				new() { Name = "Track1" },
				new() { Name = "Track2" },
				new() { Name = "Track3" },
				new() { Name = "Track4" },
				new() { Name = "Track5" }
			};

			_mockMusicRecommenderSystemService.Setup(s => s.GetRecommendedTraks(x, y, z, userId)).ReturnsAsync(recommendedTracks);

			// Act
			var result = await _musicRecommenderSystemController.GetRecommendedTraks(x, y, z);
			Assert.IsNotNull(result);

			var objectResult = result as ObjectResult;
			var statusCode = objectResult?.StatusCode;
			var tracks = objectResult?.Value as Track[];
			Assert.IsNotNull(tracks);
			Assert.AreEqual(200, statusCode);
			Assert.AreEqual(5, tracks.Length);
			Assert.AreEqual("Track1", tracks[0].Name);
		}

		[TestMethod]
		public async Task GetRecommendedTraks_ReturnsUnauthorized_WhenUserIdIsNull()
		{
			// Arrange
			_musicRecommenderSystemController.ControllerContext = new ControllerContext()
			{
				HttpContext = new DefaultHttpContext() { User = new ClaimsPrincipal() }
			};

			// Act
			var result = await _musicRecommenderSystemController.GetRecommendedTraks();

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
		}

		[TestMethod]
		public async Task GetEmotionForTrack_ReturnsEmotion()
		{
			// Arrange
			var previewUrl = "previewUrl";
			var emotion = "emotion";

			_mockMusicRecommenderSystemService.Setup(s => s.GetEmotionForTrack(previewUrl)).ReturnsAsync(emotion);

			// Act
			var result = await _musicRecommenderSystemController.GetEmotionForTrack(previewUrl);
			Assert.IsNotNull(result);

			var objectResult = result as ObjectResult;
			var statusCode = objectResult?.StatusCode;
			var emotionResult = objectResult?.Value as string;
			Assert.IsNotNull(emotionResult);
			Assert.AreEqual(200, statusCode);
			Assert.AreEqual(emotion, emotionResult);
		}

		[TestMethod]
		public async Task GetEmotionForTrack_ReturnsBadRequest_WhenPreviewUrlIsNull()
		{
			// Arrange
			string previewUrl = null;

			// Act
			var result = await _musicRecommenderSystemController.GetEmotionForTrack(previewUrl);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
		}

		[TestMethod]
		public async Task GetEmotionForTrack_ReturnsBadRequest_WhenPreviewUrlIsEmpty()
		{
			// Arrange
			string previewUrl = string.Empty;

			// Act
			var result = await _musicRecommenderSystemController.GetEmotionForTrack(previewUrl);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
		}
	}
}
