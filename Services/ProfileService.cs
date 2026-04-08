using Ads.DTO.Profile;
using Ads.Enums;
using Ads.Mappings;
using Ads.Models;
using Ads.Repositories;
using Ads.Repositories.Interfaces;
using Ads.Services.Interfaces;

namespace Ads.Services;

public class ProfileService(IUserRepository userRepository, IUnitOfWork unitOfWork, IConfiguration config) : IProfileService
{
    private readonly string _baseImageUrl = config["BaseUrls:Images"];

    public async Task<ProfileResponse> GetProfileAsync(int userId)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new Exception("User not found");
        
        var ads = (await unitOfWork.Ads.GetAllAdsByUserIdAsync(userId))
            .Select(a => a.ToResponse(_baseImageUrl))
            .ToList();

        return user.ToProfile(ads);
    }

    public async Task<ProfileResponse> UpdateAsync(int userId, UpdateProfileRequest request)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new Exception("User not found");
        
        user.Name = request.Name;
        user.Email = request.Email;
        await userRepository.SaveChangesAsync();
        var ads = (await unitOfWork.Ads.GetAllAdsByUserIdAsync(userId))
            .Select(a => a.ToResponse(_baseImageUrl))
            .ToList();
        return user.ToProfile(ads);
    }

    public async Task ChangeStatusAsync(int userId, UserStatus status)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new Exception("User not found");
        user.Status = status;
        await userRepository.SaveChangesAsync();
    }
}