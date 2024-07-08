using HarmonyOfEmotions.ApiService.Interfaces;
using HarmonyOfEmotions.Domain.RecommenderSystem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HarmonyOfEmotions.ApiService.Controllers;

/// <summary>
/// This controller is responsible for handling requests related to user track preferences.
/// </summary>
/// <param name="logger"> For logging purposes</param>
/// <param name="service"> The service that will handle the requests</param>
[ApiController, Authorize]
[Route("[controller]")]
public class UserTrackPreferencesController(
	ILogger<UserTrackPreferencesController> logger,
	IUserTrackService service
	) : ControllerBase
{

	private readonly ILogger<UserTrackPreferencesController> _logger = logger;
	private readonly IUserTrackService _service = service;

	/// <summary>
	/// Save the user's track preference (either liked or disliked).
	/// </summary>
	/// <param name="userTrackPreference"> The user's track preference</param>
	/// <returns> Ok if the user's track preference was saved</returns>
	[HttpPost("addOrUpdate")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> AddOrUpdateUserTrackPreference([FromBody] UserTrackPreference userTrackPreference)
	{
		var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		if (userId == null) return Unauthorized();
		if (userTrackPreference.TrackId == null) return BadRequest();

		await _service.AddOrUpdateUserTrackPreference(userId, userTrackPreference.TrackId, userTrackPreference.IsLiked);
		_logger.LogInformation("User track preference added or updated");
		return Ok();
	}

	/// <summary>
	/// Retrieve the user's track preferences.
	/// </summary>
	/// <returns> A list of the user's track preferences</returns>
	[HttpGet("get")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetUserTrackPreferences()
	{
		var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		if (userId == null) return Unauthorized();

		var preferences = await _service.GetUserTrackPreferences(userId);
		_logger.LogInformation("User track preferences retrieved");
		return Ok(preferences);
	}

	/// <summary>
	/// Delete the user's track preference (the track that the user liked or disliked).
	/// </summary>
	/// <param name="trackId"> The id of the track</param>
	/// <returns> Ok if the user's track preference was deleted</returns>
	[HttpDelete("delete")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> DeleteUserTrackPreference(string trackId)
	{
		var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		if (userId == null) return Unauthorized();
		if (trackId == null) return BadRequest();

		await _service.DeleteUserTrackPreference(userId, trackId);
		_logger.LogInformation("User track preference deleted");
		return Ok();
	}
}
