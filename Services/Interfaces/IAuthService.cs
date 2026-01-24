using Ads.DTO;

namespace Ads.Services.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(RegisterRequest request);
    Task<string> LoginAsync(LoginRequest request);
}