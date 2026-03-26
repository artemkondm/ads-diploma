using Ads.DTO.Ads;
using Ads.Models;

namespace Ads.Mappings;

public static class LocationMappings
{
    public static LocationResponse ToResponse(this Location location)
    {
        return new LocationResponse(
            location.City.Region.Name,
            location.City.Name,
            location.Street,
            location.House,
            location.Latitude,
            location.Longitude
        );
    }

    public static Location ToLocation(this GeocodeResult geoResult, City city)
    {
        return new Location()
        {
            City = city,
            Street = geoResult.Street,
            House = geoResult.House,
            Latitude = geoResult.Latitude,
            Longitude = geoResult.Longitude,
            YandexUri = geoResult.YandexUri,
        };
    }
}