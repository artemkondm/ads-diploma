using Ads.Models;

namespace Ads.Repositories.Interfaces;

public interface IReviewRepository
{
    Task<IEnumerable<Review>> GetAllReviewsOnModerationAsync();
    Task AddReviewAsync(Review review);
    Task<IEnumerable<Review>> GetAllReviewsBySellerIdAsync(int sellerId);
    Task<Review?> GetReviewByIdAsync(int reviewId);
}