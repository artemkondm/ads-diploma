using Ads.DTO.Ads;
using Ads.Models;

namespace Ads.Repositories.Interfaces;

public interface IAdsRepository
{
    Task AddAsync(Ad ad);
    Task DeleteAsync(Ad ad);
    Task<Ad?> GetByIdAsync(int adId);
    Task SaveChangesAsync();
}