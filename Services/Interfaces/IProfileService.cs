using Ads.DTO.Profile;

namespace Ads.Services.Interfaces;

public interface IProfileService
{
    Task<ProfileResponse> GetProfileAsync(int userId);
}