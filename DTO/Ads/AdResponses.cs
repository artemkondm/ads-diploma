using Ads.DTO.Profile;
using Ads.Models;

namespace Ads.DTO.Ads;

public record AdResponse(
    int Id,
    string Title,
    string Description,
    int Price,
    int CategoryId,
    DateTime DateCreated,
    int UserId,
    bool? IsFavorited,
    LocationResponse LocationResponse,
    List<ImageResponse> Images
    );

public record AdListResponse(
    string Title,
    int Price
    );
    
public record ImageResponse(
    string Url,
    string? ThumbnailUrl,
    bool IsMain
);