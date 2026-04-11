using Ads.Database;
using Ads.Enums;
using Ads.Models;
using Ads.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ads.Repositories;

public class ReviewRepository(AppDbContext context) : IReviewRepository
{
    public async Task<IEnumerable<Review>> GetAllReviewsOnModerationAsync()
    {
        var reviews = await context.Reviews
            .Include(r => r.Ad)
            .Include(r => r.Seller)
            .Include(r => r.Chat)
            .Where(r => r.Status == ReviewStatus.OnModeration)
            .ToListAsync();
        
        return reviews;
    }

    public async Task AddReviewAsync(Review review)
    {
        context.Add(review);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Review>> GetAllReviewsBySellerIdAsync(int sellerId)
    {
        var reviews = await context.Reviews
            .Include(r => r.Ad)
            .Include(r => r.Seller)
            .Where(r => r.SellerId == sellerId)
            .OrderByDescending(r => r.Date)
            .ToListAsync();
        return reviews;
    }
}