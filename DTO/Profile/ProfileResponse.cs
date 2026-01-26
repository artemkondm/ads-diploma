using Ads.DTO.Ads;
using Ads.Models;

namespace Ads.DTO.Profile;

public record ProfileResponse(string Name, string Email, DateTime RegistrationDate, int AdsCount, List<AdResponse> Ads);