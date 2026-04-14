using Ads.DTO;
using Ads.Enums;
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

    public async Task<IEnumerable<ReviewModerationResponse>> GetAllReviewsOnModerationAsync()
    {
        var reviews = await reviewRepository.GetAllReviewsOnModerationAsync();
        return reviews.Select(r => r.ToModerationResponse()).ToList();
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

    public async Task ChangeStatusAsync(int reviewId, ReviewStatus status)
    {
        var review = await reviewRepository.GetReviewByIdAsync(reviewId);
        if (review == null)
            throw new NullReferenceException();
        review.Status = status;
        var seller = await userRepository.GetByIdAsync(review.SellerId);
        var reviews = await reviewRepository.GetAllReviewsBySellerIdAsync(review.SellerId);
        var reviewsList = reviews.ToList();
        var sum = reviewsList
            .Where(r => r.Status == ReviewStatus.Accepted)
            .Select(r => r.Rating)
            .Sum();
        if (seller != null) seller.Rating = (double)sum / reviewsList.Count;
    }
}