using Ads.Models;

namespace Ads.Repositories.Interfaces;

public interface ICityRepository
{
    Task<City> GetOrAddAsync(string cityName, Region region);
}