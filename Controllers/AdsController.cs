using Ads.DTO.Ads;
using Ads.Mappings;
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
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateAdAsync(CreateAdRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
        
        var ad = await _adsService.CreateAsync(userId, request);
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        return CreatedAtAction(
            nameof(GetById),
            new { adId = ad.Id },
            ad.ToResponse(baseUrl)
        );
    }

    [HttpGet("{adId}")]
    public async Task<IActionResult> GetById(int adId)
    {
        var ad = await _adsService.GetByIdAsync(adId);
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        return Ok(ad.ToResponse(baseUrl));
    }

    [Authorize]
    [HttpPut("{adId}")]
    public async Task<IActionResult> UpdateAdAsync(int adId, AdUpdateRequest request)
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
        
        var ad = await _adsService.UpdateAsync(userId, adId, request);
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        return Ok(ad.ToResponse(baseUrl));
    }
    
    [Authorize]
    [HttpDelete("{adId}")]
    public async Task<IActionResult> DeleteAdAsync(int adId)
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
        await _adsService.DeleteAsync(userId, adId);
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetAds()
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var ads = await _adsService.GetAllAdsAsync(baseUrl);
        return Ok(ads);
    }
}