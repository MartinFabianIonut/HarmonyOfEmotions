using Microsoft.AspNetCore.Mvc;
using HarmonyOfEmotions.ApiService.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace HarmonyOfEmotions.ApiService.Controllers;

[ApiController, Authorize]
[Route("[controller]")]
public class ArtistInfoController(
	ILogger<ArtistInfoController> logger,
	IArtistService service
	) : ControllerBase
{

	private readonly ILogger<ArtistInfoController> _logger = logger;
	private readonly IArtistService _service = service;

	[HttpGet("GetArtistInfo")]
	public async Task<IActionResult> GetArtistInfo(string artist)
	{
		var artistInfo = await _service.GetCompleteArtistInfoAsync(artist);
		_logger.LogInformation("Artist info found");

		return Ok(artistInfo);
	}
}
