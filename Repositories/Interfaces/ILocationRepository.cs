using Ads.Models;

namespace Ads.Repositories.Interfaces;

public interface ILocationRepository
{
    Task AddLocationAsync(Location location);
}