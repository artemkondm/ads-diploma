using Ads.DTO.Profile;
using Ads.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ads.Controllers;

[ApiController]
[Route("api/profile")]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }
    
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetProfile(int userId)
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var profile =  await _profileService.GetProfileAsync(userId, baseUrl);
        return Ok(profile);
    }
}