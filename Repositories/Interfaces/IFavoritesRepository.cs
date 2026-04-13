using Ads.DTO.Ads;
using Ads.Models;

namespace Ads.Repositories.Interfaces;

public interface IFavoritesRepository
{
    Task<List<int>> GetUserFavoriteAdsAsync(int userId);
    Task<AdFavorite?> GetAsync(int userId, int adId);
    Task AddAsync(AdFavorite adFavorite);
    Task RemoveAsync(AdFavorite adFavorite);
}