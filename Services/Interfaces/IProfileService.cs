using Ads.DTO.Profile;
using Ads.Enums;
using Ads.Models;

namespace Ads.Services.Interfaces;

public interface IProfileService
{
    Task<ProfileResponse> GetProfileAsync(int userId);
    Task<ProfileResponse> UpdateAsync(int userId, UpdateProfileRequest request);
    Task ChangeStatusAsync(int userId, UserStatus status);
}