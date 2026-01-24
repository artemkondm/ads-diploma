using Ads.DTO;
using Ads.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ads.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        await _authService.RegisterAsync(registerRequest);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var token = await _authService.LoginAsync(loginRequest);
        return Ok(token);
    }
}