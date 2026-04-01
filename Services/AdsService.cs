using Ads.DTO.Ads;
using Ads.Mappings;
using Ads.Models;
using Ads.Repositories;
using Ads.Repositories.Interfaces;
using Ads.Services.Interfaces;
using Elastic.Clients.Elasticsearch;

namespace Ads.Services;

public class AdsService(IUserRepository userRepository, IGeoService geoService, IUnitOfWork unitOfWork, ISearchService searchService)
    : IAdsService
{
    public async Task<AdResponse> CreateAsync(int userId, AdCreateRequest request)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new Exception($"User with id {userId} not found");
        
        var address = $"{request.Region}, {request.City}, {request.Street}, {request.House}";
        var geoResult = await geoService.GeocodeAsync(address);
        if (geoResult is null)
            throw new Exception("Invalid address");
        
        var region = await unitOfWork.Regions.GetOrAddAsync(geoResult.Region);
        var city = await unitOfWork.Cities.GetOrAddAsync(geoResult.City, region);
        var location = geoResult.ToLocation(city);
        
        var ad = request.ToAd(user, location);
        
        await unitOfWork.Ads.AddAsync(ad);
        await unitOfWork.Locations.AddLocationAsync(location);
        await unitOfWork.SaveChangesAsync();

        await searchService.IndexAdAsync(ad);
        return ad.ToResponse();
    }

    public async Task DeleteAsync(int userId, int adId)
    {
        var ad = await unitOfWork.Ads.GetByIdAsync(adId);
        if (ad == null)
            throw new Exception("Ad not found");
        
        if (userId != ad.UserId)
            throw new UnauthorizedAccessException();
        
        await unitOfWork.Ads.DeleteAsync(ad);
    }

    public async Task<AdResponse> GetByIdAsync(int adId)
    {
        var ad = await unitOfWork.Ads.GetByIdAsync(adId);
        if (ad == null)
            throw new Exception("Ad not found");
        
        return ad.ToResponse();
    }

    public async Task<AdResponse> UpdateAsync(int userId, int adId, AdUpdateRequest request)
    {
        var ad = await unitOfWork.Ads.GetByIdAsync(adId);
        if (ad == null)
            throw new Exception("Ad not found");
        
        if (userId != ad.UserId)
            throw new UnauthorizedAccessException();

        ad.Title = request.Title;
        ad.Description = request.Description;
        ad.Price = request.Price;
        
        await unitOfWork.SaveChangesAsync();
        await searchService.IndexAdAsync(ad);
        return ad.ToResponse();
    }

    public async Task<List<AdResponse>> GetAllAdsAsync()
    {
        var ads = await unitOfWork.Ads.GetAllAdsAsync();
        var res = new List<AdResponse>();
        foreach (var ad in ads)
        {
            res.Add(ad.ToResponse());
        }
        return res;
    }
}