using Ads.DTO.Ads;
using Ads.Enums;
using Ads.Models;
using Ads.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ads.Controllers.Moderator;

public class AdsModerationController(IAdsService adsService) : ModeratorBaseController
{
    [HttpPatch("{adId}/status/{status}")]
    public async Task<IActionResult> ChangeAdStatusAsync(int adId, AdStatus status)
    {
        return Ok(await adsService.ChangeStatusAsync(adId, status));
    }

    [HttpGet]
    public async Task<IActionResult> GetAdsOnModerationAsync()
    {
        return Ok(await adsService.GetAllAdsOnModerationAsync());
    }
}