namespace Ads.DTO;

public record ReviewRequest(
    int AdId,
    string Comment,
    int Rating
    );