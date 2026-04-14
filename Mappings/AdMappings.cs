using Ads.DTO.Ads;
using Ads.Enums;
using Ads.Models;

namespace Ads.Mappings;

public static class AdMappings
{
    public static AdResponse ToResponse(this Ad ad, string baseUrl, List<int>? favoriteAdIds = null)
    {
        
        var images = ad.Images.Select(img => new ImageResponse(
            Url: $"{baseUrl}{img.Url}",
            ThumbnailUrl: img.ThumbnailUrl != null ? $"{baseUrl}{img.ThumbnailUrl}" : null,
            IsMain: img.IsMain
        )).ToList();
        return new AdResponse(
            ad.Id,
            ad.Title,
            ad.Description,
            ad.Price,
            ad.CategoryId,
            ad.DateCreated,
            ad.UserId,
            ad.Status == AdStatus.Active,
            favoriteAdIds?.Contains(ad.Id) ?? false,
            ad.Location.ToResponse(),
            images
        );
    }

    public static Ad ToAd(this AdCreateRequest request, User user, Location location)
    {
        return new Ad()
        {
            Title = request.Title,
            Description = request.Description,
            Price = request.Price,
            CategoryId = request.CategoryId,
            DateCreated = DateTime.UtcNow,
            User = user,
            Location = location
        };
    }
    public static Ad ToAd(this CreateAdRequest request, User user, Location location)
    {
        return new Ad()
        {
            Title = request.Title,
            Description = request.Description,
            Price = request.Price,
            CategoryId = request.CategoryId,
            DateCreated = DateTime.UtcNow,
            User = user,
            Location = location
        };
    }
}