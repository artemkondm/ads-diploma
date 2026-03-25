using Ads.DTO.Profile;
using Ads.Models;

namespace Ads.DTO.Ads;

public record AdResponse(
    int Id,
    string Title,
    string Description,
    int Price,
    DateTime DateCreated,
    int UserId,
    LocationResponse LocationResponse
    );

public record AdListResponse(
    string Title,
    int Price
    );