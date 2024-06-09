using HarmonyOfEmotions.ApiService.Interfaces;
using HarmonyOfEmotions.Domain.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HarmonyOfEmotions.ApiService.Controllers;

[ApiController, Authorize]
[Route("[controller]")]
public class UserTrackPreferencesController(
	ILogger<UserTrackPreferencesController> logger,
	IUserTrackService service
	) : ControllerBase
{

	private readonly ILogger<UserTrackPreferencesController> _logger = logger;
	private readonly IUserTrackService _service = service;

	[HttpPost("addOrUpdate")]
	public async Task<IActionResult> AddOrUpdateUserTrackPreference([FromBody] UserTrackPreference userTrackPreference)
	{
		var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		if (userId == null) return Unauthorized();
		if (userTrackPreference.TrackId == null) return BadRequest();

		await _service.AddOrUpdateUserTrackPreference(userId, userTrackPreference.TrackId, userTrackPreference.IsLiked);
		_logger.LogInformation("User track preference added or updated");
		return Ok();
	}

	[HttpGet("get")]
	public async Task<IActionResult> GetUserTrackPreferences()
	{
		var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		if (userId == null) return Unauthorized();

		var preferences = await _service.GetUserTrackPreferences(userId);
		_logger.LogInformation("User track preferences retrieved");
		return Ok(preferences);
	}

	[HttpDelete("delete")]
	public async Task<IActionResult> DeleteUserTrackPreference(string trackId)
	{
		var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		if (userId == null) return Unauthorized();

		await _service.DeleteUserTrackPreference(userId, trackId);
		_logger.LogInformation("User track preference deleted");
		return Ok();
	}
}
