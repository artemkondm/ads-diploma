using Ads.Enums;
using Ads.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ads.Controllers.Moderator;

public class UsersModerationController(IProfileService profileService) : ModeratorBaseController
{
    [HttpPatch("{userId}")]
    public async Task<IActionResult> ChangeStatusAsync(int userId, UserStatus status)
    {
        await profileService.ChangeStatusAsync(userId, status);
        return Ok();
    }
}