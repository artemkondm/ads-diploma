namespace Ads.DTO.Ads;

public record AdResponse(
    int Id,
    string Title,
    string Description,
    int Price,
    DateTime DateCreated,
    int UserId
    );

public record AdListResponse(
    string Title,
    int Price
    );