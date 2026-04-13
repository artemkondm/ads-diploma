using Ads.Enums;

namespace Ads.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; } = null!;
    public DateTime RegistrationDate { get; set; }
    public string PasswordHash { get; set; } = null!;
    public virtual ICollection<AdFavorite> FavoriteAds { get; set; } = new List<AdFavorite>();
    public UserRole Role { get; set; } = UserRole.User;
    public UserStatus Status { get; set; } = UserStatus.Active;
}