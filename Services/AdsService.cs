using Ads.DTO.Ads;
using Ads.Mappings;
using Ads.Models;
using Ads.Repositories;
using Ads.Repositories.Interfaces;
using Ads.Services.Interfaces;

namespace Ads.Services;

public class AdsService : IAdsService
{
    private readonly IUserRepository _userRepository;
    private readonly IGeoService _geoService;
    private readonly IUnitOfWork _unitOfWork;

    public AdsService(IUserRepository userRepository, IGeoService geoService, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _geoService = geoService;
        _unitOfWork = unitOfWork;
    }

    public async Task<AdResponse> CreateAsync(int userId, AdCreateRequest request)
    {
        var address = $"{request.Region}, {request.City}, {request.Street}, {request.House}";
        
        var geoResult = await _geoService.GeocodeAsync(address);
        if (geoResult is null)
            throw new Exception("Invalid address");
        var region = await _unitOfWork.Regions.GetOrAddAsync(geoResult.Region);
        var city = await _unitOfWork.Cities.GetOrAddAsync(geoResult.City, region);
        var location = new Location
            {
                City = city,
                Street = geoResult.Street,
                House = geoResult.House,
                Longitude = geoResult.Longitude,
                Latitude = geoResult.Latitude,
                YandexUri = geoResult.YandexUri,
            };
        
        var ad = new Ad
        {
            Title = request.Title,
            Description = request.Description,
            Price = request.Price,
            DateCreated = DateTime.UtcNow,
            User = _userRepository.GetByIdAsync(userId).Result!,
            Location = location
        };
        
        await _unitOfWork.Ads.AddAsync(ad);
        await _unitOfWork.Locations.AddLocationAsync(location);
        await _unitOfWork.SaveChangesAsync();
        return ad.ToResponse();
    }

    public async Task DeleteAsync(int userId, int adId)
    {
        var ad = await _unitOfWork.Ads.GetByIdAsync(adId);
        if (ad == null)
            throw new Exception("Ad not found");
        
        if (userId != ad.UserId)
            throw new UnauthorizedAccessException();
        
        await _unitOfWork.Ads.DeleteAsync(ad);
    }

    public async Task<AdResponse> GetByIdAsync(int adId)
    {
        var ad = await _unitOfWork.Ads.GetByIdAsync(adId);
        if (ad == null)
            throw new Exception("Ad not found");
        
        return ad.ToResponse();
    }

    public async Task<AdResponse> UpdateAsync(int userId, int adId, AdUpdateRequest request)
    {
        var ad = await _unitOfWork.Ads.GetByIdAsync(adId);
        if (ad == null)
            throw new Exception("Ad not found");
        
        if (userId != ad.UserId)
            throw new UnauthorizedAccessException();

        ad.Title = request.Title;
        ad.Description = request.Description;
        ad.Price = request.Price;
        
        await _unitOfWork.SaveChangesAsync();
        return ad.ToResponse();
    }
}