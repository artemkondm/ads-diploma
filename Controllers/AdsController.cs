using Ads.DTO.Ads;
using Ads.Models;
using Ads.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace Ads.Controllers;

[ApiController]
[Route("api/ads")]
public class AdsController : ControllerBase
{
    private readonly IAdsService _adsService;
    public AdsController(IAdsService adsService)
    {
        _adsService = adsService;
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreateAdAsync(AdCreateRequest request)
    {
        var userId = int.Parse(User.FindFirst("userId")!.Value);
        
        var ad = await _adsService.CreateAsync(userId, request);

        return CreatedAtAction(
            nameof(GetById),
            new { adId = ad.Id },
            ad
        );
    }

    [HttpGet("{adId}")]
    public async Task<IActionResult> GetById(int adId)
    {
        var ad = await _adsService.GetByIdAsync(adId);
        return Ok(ad);
    }

    [Authorize]
    [HttpPut("{adId}")]
    public async Task<IActionResult> UpdateAdAsync(int adId, AdUpdateRequest request)
    {
        var userId = int.Parse(User.FindFirst("userId")!.Value);
        
        var ad = await _adsService.UpdateAsync(userId, adId, request);
        return Ok(ad);
    }
    
    [Authorize]
    [HttpDelete("{adId}")]
    public async Task<IActionResult> DeleteAdAsync(int adId)
    {
        var userId = int.Parse(User.FindFirst("userId")!.Value);
        await _adsService.DeleteAsync(userId, adId);
        return NoContent();
    }
}