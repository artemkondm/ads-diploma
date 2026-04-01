using Ads.Models;
using Ads.Services;
using Ads.Services.Interfaces;
using Elastic.Clients.Elasticsearch;
using Microsoft.AspNetCore.Mvc;

namespace Ads.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController(ElasticsearchClient elasticClient, ISearchService searchService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        // var response = await elasticClient.SearchAsync<AdSearchModel>(s => s
        //     .Query(query => query
        //         .Match(m => m.Field(field => field.Title).Query(query))
        //     )
        // );
        var response = await searchService.SearchAdsAsync(query);
        return Ok(response);
    }
    
    [HttpPost("reindex")]
    public async Task<IActionResult> Reindex() 
    {
        await searchService.ReindexAllAdsAsync();
        return Ok("Индексация запущена");
    }
}