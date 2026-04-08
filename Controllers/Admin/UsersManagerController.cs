using Ads.DTO.Profile;
using Ads.Enums;
using Ads.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ads.Controllers.Admin;

public class UsersManagerController(IProfileService profileService) : AdminBaseController
{
    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateProfileAsync(int userId, [FromBody] UpdateProfileRequest request)
    { 
        return Ok(await profileService.UpdateAsync(userId, request));
    }

    [HttpPatch("{userId}")]
    public async Task<IActionResult> ChangeStatusAsync(int userId, UserStatus status)
    {
        await profileService.ChangeStatusAsync(userId, status);
        return Ok();
    }
}