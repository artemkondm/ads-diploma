using Ads.Enums;
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
                .Bool(b => b
                    .Must(m => m
                        .MultiMatch(mm => mm
                            .Fields(new[] { "title", "description" })
                            .Query(query)
                            .Fuzziness(new Fuzziness("AUTO"))
                        )
                    )
                    .Filter(
                        f => f.Term(t => t.Field(field => field.IsDeleted).Value(false)),
                        f => f.Term(t => t.Field(field => field.AdStatus).Value(AdStatus.Active.ToString()))
                    )
                )
            )
        );

        if (response.IsSuccess())
        {
            return response.Documents;
        }
        return [];
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
            House = ad.Location.House,
            ThumbnailUrl = ad.ThumbnailUrl
        };
        
        await elasticClient.IndexAsync(searchModel, a => a
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
        
        await elasticClient.IndexManyAsync(searchModels, "ads_index");
    }
}