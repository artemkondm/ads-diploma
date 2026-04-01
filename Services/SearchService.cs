using Ads.Models;
using Ads.Repositories;
using Ads.Repositories.Interfaces;
using Ads.Services.Interfaces;
using Elastic.Clients.Elasticsearch;
using Elastic.Esql.Extensions;

namespace Ads.Services;

public class SearchService(ElasticsearchClient elasticClient, IUnitOfWork unitOfWork) : ISearchService
{
    
    public async Task<IEnumerable<AdSearchModel>> SearchAdsAsync(string query)
    {
        var response = await elasticClient.SearchAsync<AdSearchModel>(s => s
            .Index("ads_index")
            .Query(q => q
                .MultiMatch(m => m
                    .Fields(new[] {"title", "description"})
                    .Query(query)
                    .Fuzziness(new Fuzziness("AUTO"))
                )
            )
        );

        if (response.IsSuccess())
        {
            return response.Documents;
        }
        return Enumerable.Empty<AdSearchModel>();
    }

    public async Task IndexAdAsync(Ad ad)
    {
        var searchModel = new AdSearchModel
        {
            Id = ad.Id,
            Title = ad.Title,
            Description = ad.Description,
            Price = ad.Price,
            CategoryId = ad.CategoryId,
            City = ad.Location.City.Name,
            Street = ad.Location.Street,
            House = ad.Location.House
        };
        
        var response = await elasticClient.IndexAsync(searchModel, a => a
            .Index("ads_index")
            .Id(searchModel.Id));
    }
    
    public async Task ReindexAllAdsAsync()
    {
        var ads = await unitOfWork.Ads.GetAllAdsAsync();
        var searchModels = ads.Select(ad => new AdSearchModel
        {
            Id = ad.Id,
            Title = ad.Title,
            Description = ad.Description,
            Price = ad.Price,
            CategoryId = ad.CategoryId,
            City = ad.Location.City.Name,
            Street = ad.Location.Street,
            House = ad.Location.House
        }).ToList();
        
        var response = await elasticClient.IndexManyAsync(searchModels, "ads_index");
    }
}