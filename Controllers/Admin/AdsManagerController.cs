using Ads.DTO.Ads;
using Ads.Enums;
using Ads.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ads.Controllers.Admin;

public class AdsManagerController(IAdsService adsService) : AdminBaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAllAds()
    {
        var ads = await adsService.GetAllAdsAsync();
        return Ok(ads);
    }
    
    [HttpPut("{adId}")]
    public async Task<IActionResult> UpdateAdAsync(int adId, AdUpdateRequest request)
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
        
        var ad = await adsService.UpdateAsync(userId, adId, request);
        return Ok(ad);
    }
    
    [HttpDelete("{adId}")]
    public async Task<IActionResult> DeleteAdAsync(int adId)
    {
        await adsService.DeleteAsync(adId);
        return NoContent();
    }
    
    [HttpPatch("{adId}/status/{status}")]
    public async Task<IActionResult> ChangeAdStatusAsync(int adId, AdStatus status)
    {
        return Ok(await adsService.ChangeStatusAsync(adId, status));
    }
}