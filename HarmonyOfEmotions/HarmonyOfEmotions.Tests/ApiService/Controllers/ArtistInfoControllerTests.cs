using HarmonyOfEmotions.ApiService.Controllers;
using HarmonyOfEmotions.ApiService.Interfaces;
using HarmonyOfEmotions.Domain.RecommenderSystem;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace HarmonyOfEmotions.Tests.ApiService.Controllers
{
    [TestClass]
	public class ArtistInfoControllerTests
	{
		private Mock<IArtistService> _mockArtistService;
		private ArtistInfoController _artistInfoController;
		private Mock<ILogger<ArtistInfoController>> _mockLogger;

		[TestInitialize]
		public void Setup()
		{
			_mockArtistService = new Mock<IArtistService>();
			_mockLogger = new Mock<ILogger<ArtistInfoController>>();
			_artistInfoController = new ArtistInfoController(_mockLogger.Object, _mockArtistService.Object);

			var userId = "userId";
			var mockUser = new ClaimsPrincipal(new ClaimsIdentity(
			[
				new Claim(ClaimTypes.NameIdentifier, userId)
			], "mock"));

			_artistInfoController.ControllerContext = new ControllerContext()
			{
				HttpContext = new DefaultHttpContext() { User = mockUser }
			};
		}

		[TestMethod]
		public async Task GetArtistInfo_ReturnsArtistInfo()
		{
			// Arrange
			var artistName = "Artist";
			var artistInfo = new Artist { Name = artistName, Summary = "summary", Content = "content" };

			_mockArtistService.Setup(s => s.GetCompleteArtistInfoAsync(artistName)).ReturnsAsync(artistInfo);

			// Act
			var result = await _artistInfoController.GetArtistInfo(artistName);

			var objectResult = result as ObjectResult;
			var statusCode = objectResult?.StatusCode;
			var artistInfoResult = objectResult?.Value as Artist;

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(artistInfoResult);
			Assert.AreEqual(200, statusCode);
			Assert.AreEqual(artistName, artistInfoResult!.Name);
			Assert.AreEqual("summary", artistInfoResult.Summary);
		}

		[TestMethod]
		public async Task GetArtistInfo_ReturnsNotFound_WhenArtistNotFound()
		{
			// Arrange
			var artistName = "Unknown Artist";
			_mockArtistService.Setup(s => s.GetCompleteArtistInfoAsync(artistName)).ReturnsAsync((Artist?)null);

			// Act
			var result = await _artistInfoController.GetArtistInfo(artistName);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(NotFoundResult));
		}

		[TestMethod]
		public async Task GetArtistInfo_ReturnsBadRequest_WhenArtistNameIsNull()
		{
			// Arrange
			string? artistName = null;

			// Act
			var result = await _artistInfoController.GetArtistInfo(artistName);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(BadRequestResult));
		}

		[TestMethod]
		public async Task GetArtistInfo_ReturnsBadRequest_WhenArtistNameIsEmpty()
		{
			// Arrange
			string artistName = string.Empty;

			// Act
			var result = await _artistInfoController.GetArtistInfo(artistName);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(BadRequestResult));
		}
	}
}
