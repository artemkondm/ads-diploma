using Ads.Models;

namespace Ads.Repositories.Interfaces;

public interface IRegionRepository
{
    Task<Region> GetOrAddAsync(string regionName);
}