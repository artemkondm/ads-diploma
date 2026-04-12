using Ads.DTO;
using Ads.Models;

namespace Ads.Mappings;

public static class ReviewMappings
{
    public static ReviewResponse ToResponse(this Review review)
    {
        return new ReviewResponse(
            review.ReviewerId,
            review.Reviewer.Name,
            review.AdId,
            review.Ad.Title,
            review.Comment,
            review.Date,
            review.Rating
        );
    }

    public static ReviewModerationResponse ToModerationResponse(this Review review)
    {
        return new ReviewModerationResponse(
            review.ReviewerId,
            review.Reviewer.Name,
            review.AdId,
            review.Ad.Title,
            review.Comment,
            review.Date,
            review.Rating,
            review.ChatId
        );
    }
}