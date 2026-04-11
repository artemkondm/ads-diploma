using Ads.DTO;
using Ads.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ads.Controllers.Moderator;

public class ReviewsModerationController(ReviewService reviewService) : ModeratorBaseController
{
    [HttpGet]
    public async Task<IActionResult> GetReviewsOnModerationAsync()
    {
        return Ok(await reviewService.GetAllReviewsOnModerationAsync());
    }
}