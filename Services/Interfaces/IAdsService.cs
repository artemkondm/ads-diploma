using Ads.DTO.Ads;
using Ads.Models;

namespace Ads.Services.Interfaces;

public interface IAdsService
{
    Task<Ad> CreateAsync(int userId, CreateAdRequest request);
    Task<Ad> UpdateAsync(int userId, int adId, AdUpdateRequest request);
    Task DeleteAsync(int userId, int adId);
    Task<Ad> GetByIdAsync(int adId);
    Task<List<AdResponse>> GetAllAdsAsync(string baseUrl);
}