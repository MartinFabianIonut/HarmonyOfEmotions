using Microsoft.AspNetCore.Mvc;
using HarmonyOfEmotions.ApiService.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace HarmonyOfEmotions.ApiService.Controllers;

/// <summary>
/// This controller is responsible for handling requests related to the artist info.
/// </summary>
/// <param name="logger"> For logging purposes</param>
/// <param name="service"> The service that will handle the requests</param>
[ApiController, Authorize]
[Route("[controller]")]
public class ArtistInfoController(
	ILogger<ArtistInfoController> logger,
	IArtistService service
	) : ControllerBase
{

	private readonly ILogger<ArtistInfoController> _logger = logger;
	private readonly IArtistService _service = service;

	/// <summary>
	/// Get the complete information about an artist. If just some information is found, it will be returned.
	/// </summary>
	/// <param name="artist"> The name of the artist</param>
	/// <returns> Short biography of the artist</returns>
	[HttpGet("GetArtistInfo")]
	public async Task<IActionResult> GetArtistInfo(string artist)
	{
		var artistInfo = await _service.GetCompleteArtistInfoAsync(artist);
		_logger.LogInformation("Artist info found");

		return Ok(artistInfo);
	}
}
