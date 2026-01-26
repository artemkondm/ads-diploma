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
            ad.DateCreated,
            ad.UserId
        );
    }
}