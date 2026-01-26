using Ads.DTO.Profile;
using Ads.Mappings;
using Ads.Repositories;
using Ads.Repositories.Interfaces;
using Ads.Services.Interfaces;

namespace Ads.Services;

public class ProfileService : IProfileService
{
    private readonly IUserRepository _userRepository;
    private readonly IAdsRepository _adsRepository;

    public ProfileService(IUserRepository userRepository, IAdsRepository adsRepository)
    {
        _userRepository = userRepository;
        _adsRepository = adsRepository;
    }
    
    public async Task<ProfileResponse> GetProfileAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new Exception("User not found");
        
        var ads = (await _adsRepository.GetAllAdsByUserIdAsync(userId))
            .Select(a => a.ToResponse())
            .ToList();

        return new ProfileResponse
        (
            user.Name,
            user.Email,
            user.RegistrationDate,
            ads.Count(),
            ads
        );

    }
}