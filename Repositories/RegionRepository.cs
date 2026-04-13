using Ads.Database;
using Ads.Models;
using Ads.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ads.Repositories;

public class RegionRepository : IRegionRepository
{
    private readonly AppDbContext _context;

    public RegionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Region> GetOrAddAsync(string regionName)
    {
        var region = await _context.Regions.FirstOrDefaultAsync(r => r.Name == regionName);
        if (region != null) return region;
        region = new Region() { Name = regionName };
        _context.Regions.Add(region);
        await _context.AddAsync(region);
        return region;
    }
}