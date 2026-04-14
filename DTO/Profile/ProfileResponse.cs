using Ads.DTO.Ads;
using Ads.Models;

namespace Ads.DTO.Profile;

public record ProfileResponse(string Name, string Email, DateTime RegistrationDate, int AdsCount, double Rating, List<AdResponse> Ads);
public record ShortProfileResponse(string Name);