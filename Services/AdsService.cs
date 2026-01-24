using Ads.DTO.Ads;
using Ads.Models;
using Ads.Repositories.Interfaces;
using Ads.Services.Interfaces;

namespace Ads.Services;

public class AdsService : IAdsService
{
    private readonly IAdsRepository _adsRepository;
    private readonly IUserRepository _userRepository;

    public AdsService(IAdsRepository adsRepository, IUserRepository userRepository)
    {
        _adsRepository = adsRepository;
        _userRepository = userRepository;
    }

    public async Task<AdResponse> CreateAsync(int userId, AdCreateRequest request)
    {
        var ad = new Ad
        {
            Title = request.Title,
            Description = request.Description,
            Price = request.Price,
            DateCreated = DateTime.UtcNow,
            User = _userRepository.GetByIdAsync(userId).Result!
        };
        
        await _adsRepository.AddAsync(ad);

        return new AdResponse(
            ad.Id, ad.Title, ad.Description, ad.Price, ad.DateCreated, userId
        );
    }

    public async Task DeleteAsync(int userId, int adId)
    {
        var ad = await _adsRepository.GetByIdAsync(adId);
        if (ad == null)
            throw new Exception("Ad not found");
        
        if (userId != ad.UserId)
            throw new UnauthorizedAccessException();
        
        await _adsRepository.DeleteAsync(ad);
    }

    public async Task<AdResponse> GetByIdAsync(int adId)
    {
        var ad = await _adsRepository.GetByIdAsync(adId);
        if (ad == null)
            throw new Exception("Ad not found");
        
        return new AdResponse(
            ad.Id, ad.Title, ad.Description, ad.Price, ad.DateCreated, ad.UserId);
    }

    public async Task<AdResponse> UpdateAsync(int userId, int adId, AdUpdateRequest request)
    {
        var ad = await _adsRepository.GetByIdAsync(adId);
        if (ad == null)
            throw new Exception("Ad not found");
        
        if (userId != ad.UserId)
            throw new UnauthorizedAccessException();

        ad.Title = request.Title;
        ad.Description = request.Description;
        ad.Price = request.Price;
        
        await _adsRepository.SaveChangesAsync();
        return new AdResponse(
            ad.Id, ad.Title, ad.Description, ad.Price, ad.DateCreated, ad.UserId);
    }
}