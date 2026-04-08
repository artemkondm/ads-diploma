using Ads.DTO.Ads;
using Ads.Enums;
using Ads.Models;
using Elastic.Clients.Elasticsearch.Snapshot;

namespace Ads.Services.Interfaces;

public interface IAdsService
{
    Task<AdResponse> CreateAsync(int userId, CreateAdRequest request);
    Task<AdResponse> UpdateAsync(int userId, int adId, AdUpdateRequest request);
    Task DeleteAsync(int userId, int adId);
    Task DeleteAsync(int adId);
    Task<AdResponse> GetByIdAsync(int adId);
    Task<List<AdResponse>> GetAllAdsAsync();
    Task<AdResponse> ChangeStatusAsync(int adId, AdStatus status);
}