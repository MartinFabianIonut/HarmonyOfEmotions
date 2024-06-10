using HarmonyOfEmotions.ApiService.Controllers;
using HarmonyOfEmotions.ApiService.Interfaces;
using HarmonyOfEmotions.Domain;
using HarmonyOfEmotions.Domain.DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace HarmonyOfEmotions.Tests.ApiService.Controllers
{
	[TestClass]
	public class UserTrackPreferencesControllerTests
	{
		private Mock<IUserTrackService> _mockUserTrackPreferencesService;

		private UserTrackPreferencesController _userTrackPreferencesController;
		private Mock<ILogger<UserTrackPreferencesController>> _mockLogger;

		[TestInitialize]
		public void Setup()
		{
			_mockUserTrackPreferencesService = new Mock<IUserTrackService>();
			_mockLogger = new Mock<ILogger<UserTrackPreferencesController>>();
			_userTrackPreferencesController = new UserTrackPreferencesController(_mockLogger.Object, _mockUserTrackPreferencesService.Object);

			var userId = "userId";
			var mockUser = new ClaimsPrincipal(new ClaimsIdentity(
			[
				new Claim(ClaimTypes.NameIdentifier, userId)
			], "mock"));

			_userTrackPreferencesController.ControllerContext = new ControllerContext()
			{
				HttpContext = new DefaultHttpContext() { User = mockUser }
			};
		}

		[TestMethod]
		public async Task GetUserTrackPreferences_ReturnsUserTrackPreferences()
		{
			// Arrange
			var userId = "userId";
			var userTrackPreferences = new UserTrackPreference[] {
				new() {UserId = userId, TrackId = "TrackId", IsLiked = true },
				new() {UserId = userId, IsLiked = false }
			};

			_mockUserTrackPreferencesService.Setup(s => s.GetUserTrackPreferences(userId)).ReturnsAsync(userTrackPreferences);

			// Act
			var result = await _userTrackPreferencesController.GetUserTrackPreferences();

			var objectResult = result as ObjectResult;
			var statusCode = objectResult?.StatusCode;
			var userTrackPreferencesResult = objectResult?.Value as UserTrackPreference[];

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(userTrackPreferencesResult);
			Assert.AreEqual(200, statusCode);
			Assert.AreEqual(2, userTrackPreferencesResult.Length);
			Assert.AreEqual("TrackId", userTrackPreferencesResult[0].TrackId);
			Assert.IsTrue(userTrackPreferencesResult[0].IsLiked);
		}

		[TestMethod]
		public async Task GetUserTrackPreferences_ReturnsUnauthorized_WhenUserIdIsNull()
		{
			// Arrange
			_userTrackPreferencesController.ControllerContext = new ControllerContext()
			{
				HttpContext = new DefaultHttpContext() { User = new ClaimsPrincipal() }
			};

			// Act
			var result = await _userTrackPreferencesController.GetUserTrackPreferences();

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
		}

		[TestMethod]
		public async Task UpdateUserTrackPreference_ReturnsUpdatedUserTrackPreference()
		{
			// Arrange
			var userId = "userId";
			var trackId = "TrackId";
			var isLiked = true;
			var userTrackPreference = new UserTrackPreference { UserId = userId, TrackId = trackId, IsLiked = isLiked };

			_mockUserTrackPreferencesService.Setup(s => s.AddOrUpdateUserTrackPreference(userId, trackId, isLiked)).Returns(Task.CompletedTask);
			// Act
			var result = await _userTrackPreferencesController.AddOrUpdateUserTrackPreference(userTrackPreference);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(OkResult));
		}

		[TestMethod]
		public async Task UpdateUserTrackPreference_ReturnsBadRequest_WhenTrackIdIsNull()
		{
			// Arrange
			var userId = "userId";
			var isLiked = true;
			var userTrackPreference = new UserTrackPreference { UserId = userId, IsLiked = isLiked };

			// Act
			var result = await _userTrackPreferencesController.AddOrUpdateUserTrackPreference(userTrackPreference);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(BadRequestResult));
		}

		[TestMethod]
		public async Task UpdateUserTrackPreference_ReturnsUnauthorized_WhenUserIdIsNull()
		{
			// Arrange
			var trackId = "TrackId";
			var isLiked = true;
			var userTrackPreference = new UserTrackPreference { TrackId = trackId, IsLiked = isLiked };

			_userTrackPreferencesController.ControllerContext = new ControllerContext()
			{
				HttpContext = new DefaultHttpContext() { User = new ClaimsPrincipal() }
			};

			// Act
			var result = await _userTrackPreferencesController.AddOrUpdateUserTrackPreference(userTrackPreference);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
		}

		[TestMethod]
		public async Task DeleteUserTrackPreference_ReturnsDeletedUserTrackPreference()
		{
			// Arrange
			var userId = "userId";
			var trackId = "TrackId";

			_mockUserTrackPreferencesService.Setup(s => s.DeleteUserTrackPreference(userId, trackId)).Returns(Task.CompletedTask);

			// Act
			var result = await _userTrackPreferencesController.DeleteUserTrackPreference(trackId);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(OkResult));
		}

		[TestMethod]
		public async Task DeleteUserTrackPreference_ReturnsBadRequest_WhenTrackIdIsNull()
		{
			// Arrange
			string? trackId = null;

			// Act
			var result = await _userTrackPreferencesController.DeleteUserTrackPreference(trackId);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(BadRequestResult));
		}

		[TestMethod]
		public async Task DeleteUserTrackPreference_ReturnsUnauthorized_WhenUserIdIsNull()
		{
			// Arrange
			var trackId = "TrackId";

			_userTrackPreferencesController.ControllerContext = new ControllerContext()
			{
				HttpContext = new DefaultHttpContext() { User = new ClaimsPrincipal() }
			};

			// Act
			var result = await _userTrackPreferencesController.DeleteUserTrackPreference(trackId);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
		}
	}
}
