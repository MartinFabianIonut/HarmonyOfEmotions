using Microsoft.AspNetCore.Mvc;
using HarmonyOfEmotions.ApiService.Interfaces;
using SpotifyAPI.Web;
using Microsoft.AspNetCore.Authorization;
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

	[HttpGet("GetRecommendedTraks")]
	public async Task<IActionResult> GetRecommendedTraks(float x, float y, float z)
	{
		var recommendedTracks = await _service.GetRecommendedTraks(x, y, z);
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
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetEmotionForTrack(string previewUrl)
	{
		var emotion = await _service.GetEmotionForTrack(previewUrl);
		_logger.LogInformation("Emotion found");

		return Ok(emotion);
	}
}
