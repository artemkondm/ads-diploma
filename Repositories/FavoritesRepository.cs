using Ads.Database;
using Ads.DTO.Ads;
using Ads.Mappings;
using Ads.Models;
using Ads.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ads.Repositories;

public class FavoritesRepository(AppDbContext context) : IFavoritesRepository
{
    public async Task<List<int>> GetUserFavoriteAdsAsync(int userId)
    {
        return await context.FavoriteAds
            .Where(f => f.UserId == userId)
            .OrderByDescending(f => f.DateAdded)
            .Select(f => f.AdId)
            .ToListAsync();
    }

    public async Task<AdFavorite?> GetAsync(int userId, int adId)
    {
        return await context.FavoriteAds.FirstOrDefaultAsync(f => f.AdId == adId && f.UserId == userId);
    }

    public async Task AddAsync(AdFavorite adFavorite)
    {
        await context.FavoriteAds.AddAsync(adFavorite);
    }

    public async Task RemoveAsync(AdFavorite adFavorite)
    {
        context.FavoriteAds.Remove(adFavorite);
    }
}