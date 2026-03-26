using Ads.DTO.Ads;
using Ads.Models;

namespace Ads.Mappings;

public static class AdMappings
{
    public static AdResponse ToResponse(this Ad ad)
    {
        return new AdResponse(
            ad.Id,
            ad.Title,
            ad.Description,
            ad.Price,
            ad.CategoryId,
            ad.DateCreated,
            ad.UserId,
            ad.Location.ToResponse()
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
}