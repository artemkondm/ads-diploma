using Ads.Database;
using Ads.Models;
using Ads.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ads.Repositories;

public class AdsRepository : IAdsRepository
{
    private readonly AppDbContext _context;

    public AdsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Ad ad)
    {
        _context.Add(ad);
    }

    public async Task DeleteAsync(Ad ad)
    {
        _context.Remove(ad);
        await _context.SaveChangesAsync();
    }

    public async Task<Ad?> GetByIdAsync(int adId)
    {
        return await _context.Ads
            .Include(a => a.Location)
                .ThenInclude(l => l.City)
                    .ThenInclude(c => c.Region)
            .FirstOrDefaultAsync(a => a.Id == adId);
    }

    public async Task<List<Ad>> GetAllAdsByUserIdAsync(int userId) 
        => await _context.Ads.Where(a => a.UserId == userId).ToListAsync();
    
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}