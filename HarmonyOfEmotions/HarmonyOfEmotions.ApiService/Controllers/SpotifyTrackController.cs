using Microsoft.AspNetCore.Mvc;
using HarmonyOfEmotions.ApiService.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace HarmonyOfEmotions.ApiService.Controllers;

/// <summary>
/// This controller is responsible for handling requests related to Spotify tracks.
/// </summary>
/// <param name="logger">For logging purposes</param>
/// <param name="service">The service that will handle the requests</param>
[ApiController, Authorize]
[Route("[controller]")]
public class SpotifyTrackController(
	ILogger<SpotifyTrackController> logger,
	ISpotifyService service
	) : ControllerBase
{

	private readonly ILogger<SpotifyTrackController> _logger = logger;
	private readonly ISpotifyService _service = service;

	/// <summary>
	/// This method is responsible for getting the top tracks for a given artist.
	/// </summary>
	/// <param name="artistId">This must be the artist's id</param>
	/// <returns></returns>
	[HttpGet("GetTopTracksForArtist")]
	public async Task<IActionResult> GetTopTracksForArtist(string artistId)
	{
		var topTracks = await _service.GetTopTracksForArtist(artistId);
		_logger.LogInformation("Top tracks for artistId");

		return Ok(topTracks);
	}

	/// <summary>
	/// Search for tracks by keyword.
	/// </summary>
	/// <param name="keyword">A keyword to search for</param>
	/// <returns></returns>
	[HttpGet("SearchTraksByKeyword")]
	public async Task<IActionResult> SearchTraksByKeyword(string keyword)
	{
		var tracks = await _service.SearchTraksByKeyword(keyword);
		_logger.LogInformation("Tracks found");

		return Ok(tracks);
	}
}
