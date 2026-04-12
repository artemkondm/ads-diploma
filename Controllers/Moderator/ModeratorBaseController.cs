using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ads.Controllers.Moderator;

[ApiController]
[Route("api/moderation/[controller]")]
[Authorize(Roles = "Moderator, Admin")]
public abstract class ModeratorBaseController : ControllerBase
{
    
}