using Ads.DTO.Ads;
using Ads.Models;

namespace Ads.Repositories.Interfaces;

public interface IAdsRepository
{
    Task AddAsync(Ad ad);
    Task DeleteAsync(Ad ad);
    Task<Ad?> GetByIdAsync(int adId);
    Task<List<Ad>> GetAllAdsByUserIdAsync(int userId);
    Task<List<Ad>> GetAllAdsAsync();
    Task<List<Ad>> GetAllAdsOnModerationAsync();
    Task<List<Ad>> GetAdsByIdsAsync(IEnumerable<int> adIds);
    Task SaveChangesAsync();
}