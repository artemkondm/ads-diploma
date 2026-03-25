namespace Ads.DTO.Ads;

public class GeocodeResult
{
    public string Region { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string House { get; set; } = null!;
    public string? YandexUri { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }
}