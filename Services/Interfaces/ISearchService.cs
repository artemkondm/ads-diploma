using Ads.Models;

namespace Ads.Services.Interfaces;

public interface ISearchService
{
    Task<IEnumerable<AdSearchModel>> SearchAdsAsync(string query);
    Task IndexAdAsync(Ad ad);
    Task ReindexAllAdsAsync();
}