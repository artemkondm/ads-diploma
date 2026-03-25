using Ads.Database;
using Ads.Models;
using Ads.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ads.Repositories;

public class CityRepository : ICityRepository
{
    private readonly AppDbContext _context;

    public CityRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<City> GetOrAddAsync(string cityName, Region region)
    {
        var city = await _context.Cities.FirstOrDefaultAsync(c => c.Name == cityName && c.RegionId == region.Id);
        if (city == null)
        {
            city = new City()
            {
                Name = cityName,
                Region = region
            };
            await _context.AddAsync(city);
        }
        return city;
    }
}