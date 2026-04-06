using Ads.DTO.Profile;
using Ads.Mappings;
using Ads.Repositories;
using Ads.Repositories.Interfaces;
using Ads.Services.Interfaces;

namespace Ads.Services;

public class ProfileService(IUserRepository userRepository, IUnitOfWork unitOfWork) : IProfileService
{
    public async Task<ProfileResponse> GetProfileAsync(int userId, string baseUrl)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new Exception("User not found");
        
        var ads = (await unitOfWork.Ads.GetAllAdsByUserIdAsync(userId))
            .Select(a => a.ToResponse(baseUrl))
            .ToList();

        return user.ToProfile(ads);
    }
}