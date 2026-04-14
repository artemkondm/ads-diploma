using Ads.DTO.Ads;
using Ads.Mappings;
using Ads.Models;
using Ads.Repositories.Interfaces;
using Ads.Services.Interfaces;

namespace Ads.Services;

public class FavoritesService(IUnitOfWork unitOfWork, IConfiguration config) : IFavoritesService
{
    private readonly string _baseImageUrl = config["BaseUrls:Images"];
    public async Task ToggleFavoriteAsync(int userId, int adId)
    {
        var adExists = await unitOfWork.Ads.GetByIdAsync(adId);
        if (adExists == null)
        {
            throw new Exception("Объявление не найдено или было удалено.");
        }
        var existing = await unitOfWork.Favorites.GetAsync(userId, adId);

        if (existing == null)
            await unitOfWork.Favorites.AddAsync(new AdFavorite{UserId = userId, AdId = adId, DateAdded = DateTime.UtcNow});
        else
            await unitOfWork.Favorites.RemoveAsync(existing);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<List<int>> GetUserFavoriteAdsAsync(int userId)
    {
        return await unitOfWork.Favorites.GetUserFavoriteAdsAsync(userId);
    }
}