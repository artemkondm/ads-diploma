using Ads.DTO.Profile;
using Ads.Services.Interfaces;
using Elastic.Clients.Elasticsearch.Core.Search;
using Microsoft.AspNetCore.Authorization;
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
        var profile =  await _profileService.GetProfileAsync(userId);
        return Ok(profile);
    }

    [Authorize]
    [HttpPut("/edit")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
        return Ok(await _profileService.UpdateAsync(userId, request));
    }
    
}