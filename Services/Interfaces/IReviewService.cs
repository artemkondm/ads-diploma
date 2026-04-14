using Ads.DTO;
using Ads.Enums;
using Ads.Models;

namespace Ads.Services.Interfaces;

public interface IReviewService
{
    Task<IEnumerable<ReviewResponse>> GetAllReviewsOnUserAsync(int sellerId);
    Task<IEnumerable<ReviewModerationResponse>> GetAllReviewsOnModerationAsync();
    Task AddReviewAsync(int userId, ReviewRequest review);
    Task ChangeStatusAsync(int reviewId, ReviewStatus status);
}