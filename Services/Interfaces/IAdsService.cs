using Ads.DTO.Ads;
using Ads.Models;

namespace Ads.Services.Interfaces;

public interface IAdsService
{
    Task<AdResponse> CreateAsync(int userId, AdCreateRequest request);
    Task<AdResponse> UpdateAsync(int userId, int adId, AdUpdateRequest request);
    Task DeleteAsync(int userId, int adId);
    Task<AdResponse> GetByIdAsync(int adId);
}