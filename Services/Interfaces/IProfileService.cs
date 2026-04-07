using Ads.DTO.Profile;
using Ads.Models;

namespace Ads.Services.Interfaces;

public interface IProfileService
{
    Task<ProfileResponse> GetProfileAsync(int userId);
    Task<ProfileResponse> UpdateAsync(int userId, UpdateProfileRequest request);
}