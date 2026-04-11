using Ads.DTO;
using Ads.Mappings;
using Ads.Models;
using Ads.Repositories.Interfaces;
using Ads.Services.Interfaces;

namespace Ads.Services;

public class ReviewService(IReviewRepository reviewRepository, IUnitOfWork unitOfWork, IUserRepository userRepository) : IReviewService
{
    public async Task<IEnumerable<ReviewResponse>> GetAllReviewsOnUserAsync(int sellerId)
    {
        var reviews = await reviewRepository.GetAllReviewsBySellerIdAsync(sellerId);
        return reviews.Select(r => r.ToResponse()).ToList();
    }

    public async Task<IEnumerable<ReviewResponse>> GetAllReviewsOnModerationAsync()
    {
        var reviews = await reviewRepository.GetAllReviewsOnModerationAsync();
        return reviews.Select(r => r.ToResponse()).ToList();
    }

    public async Task AddReviewAsync(int userId, ReviewRequest reviewRequest)
    {
        var ad = await unitOfWork.Ads.GetByIdAsync(reviewRequest.AdId);
        var chat = await unitOfWork.Chats
            .FindAsync(chat => chat.BuyerId == userId && chat.SellerId == ad.UserId);
        var reviewer = await userRepository.GetByIdAsync(userId);
        var review = new Review()
        {
            Ad = ad,
            Reviewer = reviewer,
            Seller = ad.User,
            Chat = chat.FirstOrDefault(),
            Comment = reviewRequest.Comment,
            Date = DateTime.UtcNow,
            Rating = reviewRequest.Rating
        };
        await reviewRepository.AddReviewAsync(review);
    }
    
}