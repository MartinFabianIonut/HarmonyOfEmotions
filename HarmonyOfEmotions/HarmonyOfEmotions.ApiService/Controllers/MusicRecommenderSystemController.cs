using HarmonyOfEmotions.ApiService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HarmonyOfEmotions.ApiService.Controllers;

/// <summary>
/// This controller is responsible for handling requests related to the music recommender system.
/// </summary>
/// <param name="logger">For logging purposes</param>
/// <param name="service">The service that will handle the requests 
[ApiController, Authorize]
[Route("[controller]")]
public class MusicRecommenderSystemController(
	ILogger<MusicRecommenderSystemController> logger,
	IRecommenderService service
	) : ControllerBase
{

	private readonly ILogger<MusicRecommenderSystemController> _logger = logger;
	private readonly IRecommenderService _service = service;

	/// <summary>
	/// Get a list of 5 recommended tracks based on the user's emotional state.
	/// </summary>
	/// <param name="x"> The valence of the user's emotional state</param>
	/// <param name="y"> The arousal of the user's emotional state</param>
	/// <param name="z"> The danceability of the user's emotional state</param>
	/// <returns> A list of 5 recommended tracks</returns>
	[HttpGet("GetRecommendedTraks")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	[ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
	public async Task<IActionResult> GetRecommendedTraks(float x = 0, float y = 0, float z = 0)
	{
		var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		if (userId == null)
		{
			return Unauthorized();
		}

		var recommendedTracks = await _service.GetRecommendedTraks(x, y, z, userId);
		_logger.LogInformation("Tracks recommended");

		return Ok(recommendedTracks);
	}

	/// <summary>
	/// This method is responsible for getting the emotion for a given track.
	/// </summary>
	/// <param name="previewUrl">The preview url of the track</param>
	/// <returns>A string representing the emotion</returns>
	[HttpGet("GetEmotionForTrack")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	[ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
	public async Task<IActionResult> GetEmotionForTrack(string previewUrl)
	{
		if (string.IsNullOrEmpty(previewUrl))
		{
			return BadRequest("Preview url is required");
		}

		var emotion = await _service.GetEmotionForTrack(previewUrl);
		_logger.LogInformation("Emotion found");

		return Ok(emotion);
	}
}
