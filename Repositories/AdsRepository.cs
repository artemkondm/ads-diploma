using Ads.Database;
using Ads.Models;
using Ads.Repositories.Interfaces;

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
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Ad ad)
    {
        _context.Remove(ad);
        await _context.SaveChangesAsync();
    }
    
    public async Task<Ad?> GetByIdAsync(int adId) => await _context.Ads.FindAsync(adId);
    
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}