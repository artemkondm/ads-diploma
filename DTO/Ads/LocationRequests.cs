namespace Ads.DTO.Ads;

public record LocationCreateRequest(string Region, string City, string Street, string House, double Latitude, double Longitude);
