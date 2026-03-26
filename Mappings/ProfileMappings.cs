using Ads.DTO.Ads;
using Ads.DTO.Profile;
using Ads.Models;

namespace Ads.Mappings;

public static class ProfileMappings
{
    public static ProfileResponse ToProfile(this User user, List<AdResponse> ads)
    {
        return new ProfileResponse
        (
            user.Name,
            user.Email,
            user.RegistrationDate,
            ads.Count,
            ads
        );
    }
}