namespace Ads.DTO;

public record ReviewResponse(
    int ReviewerId,
    string ReviewerName,
    int AdId,
    string AdTitle,
    string Comment,
    DateTime Date,
    int Rating
    );