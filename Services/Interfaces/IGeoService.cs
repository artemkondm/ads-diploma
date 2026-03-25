using Ads.DTO.Ads;

namespace Ads.Services.Interfaces;

public interface IGeoService
{
    Task<GeocodeResult> GeocodeAsync(string address);
}