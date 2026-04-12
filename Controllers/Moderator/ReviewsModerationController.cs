using Ads.DTO;
using Ads.Services;
using Ads.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ads.Controllers.Moderator;

public class ReviewsModerationController(IReviewService reviewService, IChatService chatService) : ModeratorBaseController
{
    [HttpGet]
    public async Task<IActionResult> GetReviewsOnModerationAsync()
    {
        return Ok(await reviewService.GetAllReviewsOnModerationAsync());
    }
    
    [HttpGet("{chatId}/chatMessages")]
    public async Task<IActionResult> GetMessages(int chatId)
    {
        try
        {
            var messages = await chatService.GetChatMessagesAsync(chatId);
            return Ok(messages);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }
}