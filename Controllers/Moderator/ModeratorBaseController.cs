using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ads.Controllers.Moderator;

[ApiController]
[Route("api/moderation/[controller]")]
[Authorize(Roles = "Moderator")]
public abstract class ModeratorBaseController : ControllerBase
{
    
}