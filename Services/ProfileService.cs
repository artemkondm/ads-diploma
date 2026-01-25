using Ads.DTO.Profile;
using Ads.Repositories;
using Ads.Services.Interfaces;

namespace Ads.Services;

public class ProfileService : IProfileService
{
    private readonly UserRepository _userRepository;
    private readonly AdsRepository _adsRepository;

    public ProfileService(UserRepository userRepository, AdsRepository adsRepository)
    {
        _userRepository = userRepository;
        _adsRepository = adsRepository;
    }
    
    public async Task<ProfileResponse> GetProfileAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new Exception("User not found");
        
        var ads = await _adsRepository.GetAllAdsByUserIdAsync(userId);

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