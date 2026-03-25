using Ads.Database;
using Ads.Models;
using Ads.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ads.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly AppDbContext _context;

    public LocationRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task AddLocationAsync(Location location)
    {
        _context.Add(location);
        await _context.SaveChangesAsync();
    }
}