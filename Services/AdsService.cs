using Ads.DTO.Ads;
using Ads.Enums;
using Ads.Mappings;
using Ads.Models;
using Ads.Repositories;
using Ads.Repositories.Interfaces;
using Ads.Services.Interfaces;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Snapshot;

namespace Ads.Services;

public class AdsService(IUserRepository userRepository, IGeoService geoService, IUnitOfWork unitOfWork, 
    ISearchService searchService, IImageService imageService, IConfiguration config)
    : IAdsService
{
    private readonly string _baseImageUrl = config["BaseUrls:Images"];
    public async Task<AdResponse> CreateAsync(int userId, CreateAdRequest request)
    {
        const int maxImages = 10;

        if (request.Images.Count > maxImages)
        {
            throw new BadHttpRequestException($"Нельзя загрузить более {maxImages} изображений.");
        }
        
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
        
        foreach (var image in request.Images)
        {
            ValidateImage(image);
            var isMain = ad.Images.Count == 0;
            var imagePaths =  await imageService.UploadImageAsync(image);
            var adImage = new AdImage
            {
                Url = imagePaths.OriginalUrl,
                ThumbnailUrl = imagePaths.ThumbnailUrl,
                IsMain = isMain
            };
            ad.Images.Add(adImage);
            if (isMain)
            {
                ad.ThumbnailUrl = imagePaths.ThumbnailUrl;
            }
        }
        
        await unitOfWork.Ads.AddAsync(ad);
        await unitOfWork.Locations.AddLocationAsync(location);
        await unitOfWork.SaveChangesAsync();

        await searchService.IndexAdAsync(ad);
        return ad.ToResponse(_baseImageUrl);
    }

    public async Task DeleteAsync(int userId, int adId)
    {
        var ad = await unitOfWork.Ads.GetByIdAsync(adId);
        if (ad == null)
            throw new Exception("Ad not found");
        
        if (userId != ad.UserId)
            throw new UnauthorizedAccessException();
        
        ad.IsDeleted = true;
        await unitOfWork.Ads.SaveChangesAsync();
    }

    public async Task DeleteAsync(int adId)
    {
        var ad = await unitOfWork.Ads.GetByIdAsync(adId);
        if (ad == null)
            throw new Exception("Ad not found");
        ad.IsDeleted = true;
        await unitOfWork.Ads.SaveChangesAsync();
    }
    public async Task<AdResponse> GetByIdAsync(int adId)
    {
        var ad = await unitOfWork.Ads.GetByIdAsync(adId);
        if (ad == null)
            throw new Exception("Ad not found");
        
        return ad.ToResponse(_baseImageUrl);
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
        return ad.ToResponse(_baseImageUrl);
    }

    public async Task<List<AdResponse>> GetAllAdsAsync()
    {
        var ads = await unitOfWork.Ads.GetAllAdsAsync();
        return ads.Select(ad => ad.ToResponse(_baseImageUrl)).ToList();
    }

    public async Task<AdResponse> ChangeStatusAsync(int adId, AdStatus status)
    {
        var ad = await unitOfWork.Ads.GetByIdAsync(adId);
        ad.Status = status;
        await unitOfWork.Ads.SaveChangesAsync();
        await searchService.IndexAdAsync(ad);
        return ad.ToResponse(_baseImageUrl);
    }

    public async Task<AdResponse> MakeInactiveAsync(int userId, int adId)
    {
        var ad = await unitOfWork.Ads.GetByIdAsync(adId);
        if (ad == null)
            throw new Exception("Ad not found");
        
        if (userId != ad.UserId)
            throw new UnauthorizedAccessException();
        ad.Status = AdStatus.Inactive;
        await unitOfWork.Ads.SaveChangesAsync();
        return ad.ToResponse(_baseImageUrl);
    }
    private void ValidateImage(IFormFile file)
    {
        var allowedExtensions = new List<string> { ".jpg", ".png", ".jpeg" };
        var extension = Path.GetExtension(file.FileName).ToLower();
        
        if (!allowedExtensions.Contains(extension))
            throw new BadHttpRequestException("Допустимы только изображения .jpg, .jpeg, .png");
        
        const long maxFileSize = 8 * 1024 * 1024;
        if (file.Length > maxFileSize)
            throw new BadHttpRequestException("Файл слишком большой. Максимальный размер — 8 МБ");
    }

    public async Task<List<AdResponse>> GetAllAdsOnModerationAsync()
    {
        var ads = await unitOfWork.Ads.GetAllAdsOnModerationAsync();
        return ads.Select(ad => ad.ToResponse(_baseImageUrl)).ToList();
    }
}