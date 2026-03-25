using Ads.DTO.Profile;
using Ads.Mappings;
using Ads.Repositories;
using Ads.Repositories.Interfaces;
using Ads.Services.Interfaces;

namespace Ads.Services;

public class ProfileService : IProfileService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProfileService(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ProfileResponse> GetProfileAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new Exception("User not found");
        
        var ads = (await _unitOfWork.Ads.GetAllAdsByUserIdAsync(userId))
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