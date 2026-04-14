using Ads.DTO.Ads;
using Ads.Enums;
using Ads.Mappings;
using Ads.Models;
using Ads.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace Ads.Controllers;

[ApiController]
[Route("api/ads")]
public class AdsController(IAdsService adsService, IFavoritesService favoritesService) : ControllerBase
{
    [Authorize]
    [HttpPost("create")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateAdAsync(CreateAdRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
        
        var ad = await adsService.CreateAsync(userId, request);
        return CreatedAtAction(
            nameof(GetById),
            new { adId = ad.Id },
            ad
        );
    }

    [HttpGet("{adId}")]
    public async Task<IActionResult> GetById(int adId)
    {
        var ad = await adsService.GetByIdAsync(adId);
        return Ok(ad);
    }
    [Authorize]
    [HttpGet("favorites")]
    public async Task<IActionResult> GetFavoriteAds()
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
        var favIds = await favoritesService.GetUserFavoriteAdsAsync(userId);
        return Ok(await adsService.GetAdsByIdsAsync(favIds));
    }

    [Authorize]
    [HttpPost("favorite/{adId}")]
    public async Task<IActionResult> ToggleFavorite(int adId)
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
        await favoritesService.ToggleFavoriteAsync(userId, adId);
        return Ok();
    }

    [Authorize]
    [HttpPut("{adId}")]
    public async Task<IActionResult> UpdateAdAsync(int adId, AdUpdateRequest request)
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
        
        var ad = await adsService.UpdateAsync(userId, adId, request);
        return Ok(ad);
    }
    
    [Authorize]
    [HttpDelete("{adId}")]
    public async Task<IActionResult> DeleteAdAsync(int adId)
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
        await adsService.DeleteAsync(userId, adId);
        return NoContent();
    }

    [Authorize]
    [HttpPatch("{adId}/change-status")]
    public async Task<IActionResult> MakeInactiveAsync(int adId)
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
        return Ok(await adsService.MakeInactiveAsync(userId, adId));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAds()
    {
        var ads = await adsService.GetAllAdsAsync();
        return Ok(ads);
    }
}