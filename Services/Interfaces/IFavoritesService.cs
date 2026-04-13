using Ads.DTO.Ads;
using Ads.Models;

namespace Ads.Services.Interfaces;

public interface IFavoritesService
{
    Task ToggleFavoriteAsync(int userId, int adId);
    Task<List<int>> GetUserFavoriteAdsAsync(int userId);
}