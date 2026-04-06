using Ads.DTO.Ads;
using Ads.Mappings;
using Ads.Models;
using Ads.Repositories;
using Ads.Repositories.Interfaces;
using Ads.Services.Interfaces;
using Elastic.Clients.Elasticsearch;

namespace Ads.Services;

public class AdsService(IUserRepository userRepository, IGeoService geoService, IUnitOfWork unitOfWork, ISearchService searchService, IImageService imageService)
    : IAdsService
{
    public async Task<Ad> CreateAsync(int userId, CreateAdRequest request)
    {
        const int maxImages = 10;

        if (request.Images.Count > maxImages)
        {
            throw new BadHttpRequestException($"Нельзя загрузить более {maxImages} изображений.");
        }
        
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new Exception($"User with id {userId} not found");
        
        // ValidateImage(request.Image);
        // string thumbUrl = await imageService.UploadImageAsync(request.Image);
        
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
            var imagePaths =  await imageService.UploadImageAsync(image, isMain);
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
        return ad;
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

    public async Task<Ad> GetByIdAsync(int adId)
    {
        var ad = await unitOfWork.Ads.GetByIdAsync(adId);
        if (ad == null)
            throw new Exception("Ad not found");
        
        return ad;
    }

    public async Task<Ad> UpdateAsync(int userId, int adId, AdUpdateRequest request)
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
        return ad;
    }

    public async Task<List<AdResponse>> GetAllAdsAsync(string baseUrl)
    {
        var ads = await unitOfWork.Ads.GetAllAdsAsync();
        var res = new List<AdResponse>();
        foreach (var ad in ads)
        {
            res.Add(ad.ToResponse(baseUrl));
        }
        return res;
    }

    private void ValidateImage(IFormFile file)
    {
        var allowedExtensions = new List<string> { ".jpg", ".png", ".jpeg" };
        var extension = Path.GetExtension(file.FileName).ToLower();
        
        if (!allowedExtensions.Contains(extension))
            throw new BadHttpRequestException("Допустимы только изображения .jpg, .jpeg, .png");
        
        long maxFileSize = 8 * 1024 * 1024;
        if (file.Length > maxFileSize)
            throw new BadHttpRequestException("Файл слишком большой. Максимальный размер — 8 МБ");
    }
}