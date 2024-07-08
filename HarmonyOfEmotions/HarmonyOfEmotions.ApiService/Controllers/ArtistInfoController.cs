using HarmonyOfEmotions.ApiService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HarmonyOfEmotions.ApiService.Controllers;

/// <summary>
/// This controller is responsible for handling requests related to the artist info.
/// </summary>
/// <param name="logger"> For logging purposes</param>
/// <param name="service"> The service that will handle the requests</param>
[ApiController]
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
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetArtistInfo(string artist)
	{
		if (string.IsNullOrWhiteSpace(artist))
		{
			_logger.LogInformation("Artist name is empty");
			return BadRequest();
		}
		var artistInfo = await _service.GetCompleteArtistInfoAsync(artist);
		if (artistInfo == null) 
		{
			_logger.LogInformation("Artist info not found");
			return NotFound();
		}
		_logger.LogInformation("Artist info found");

		return Ok(artistInfo);
	}
}
