using Ads.DTO;
using Ads.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ads.Controllers;

[ApiController]
[Route("/api/reviews")]
public class ReviewController(IReviewService reviewService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllReviewsOnUserAsync(int userId)
    {
        return Ok(await reviewService.GetAllReviewsOnUserAsync(userId));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddReview(ReviewRequest review)
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
        await reviewService.AddReviewAsync(userId, review);
        return Ok();
    }
}