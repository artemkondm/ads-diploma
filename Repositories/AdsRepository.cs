using Ads.Database;
using Ads.Enums;
using Ads.Models;
using Ads.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ads.Repositories;

public class AdsRepository(AppDbContext context) : IAdsRepository
{
    public async Task AddAsync(Ad ad)
    {
        context.Add(ad);
    }

    public async Task DeleteAsync(Ad ad)
    {
        context.Remove(ad);
        await context.SaveChangesAsync();
    }

    public async Task<Ad?> GetByIdAsync(int adId)
    {
        return await context.Ads
            .Include(a => a.Images)
            .Include(a => a.Location)
                .ThenInclude(l => l.City)
                    .ThenInclude(c => c.Region)
            .Where(a => a.Status == AdStatus.Active)
            .FirstOrDefaultAsync(a => a.Id == adId);
    }

    public async Task<List<Ad>> GetAllAdsByUserIdAsync(int userId) 
        => await context.Ads
            .Where(a => a.UserId == userId)
            .Include(a => a.Location)
            .ThenInclude(l => l.City)
            .ThenInclude(c => c.Region)
            .Include(a => a.Images)
            .ToListAsync();
    
    public async Task SaveChangesAsync() => await context.SaveChangesAsync();
    
    public async Task<List<Ad>> GetAllAdsAsync() => await context.Ads
        .Include(a => a.Location)
        .ThenInclude(l => l.City)
        .ThenInclude(c => c.Region)
        .Include(a => a.Images)
        .Where(a => a.Status == AdStatus.Active)
        .ToListAsync();
    
    public async Task<List<Ad>> GetAllAdsOnModerationAsync() => await context.Ads
        .Include(a => a.Location)
        .ThenInclude(l => l.City)
        .ThenInclude(c => c.Region)
        .Include(a => a.Images)
        .Where(a => a.Status == AdStatus.OnModeration)
        .ToListAsync();
    
    public async Task<List<Ad>> GetAdsByIdsAsync(IEnumerable<int> adIds)
    {
        return await context.Ads
            .AsNoTracking()
            .Include(a => a.Images)
            .Include(a => a.Location)
            .ThenInclude(l => l.City)
            .ThenInclude(c => c.Region)
            .Where(ad => adIds.Contains(ad.Id))
            .ToListAsync();
    }
}