namespace Ads.Models;

public class AdFavorite
{
    public int UserId { get; set; }
    public User User { get; set; }
    
    public int AdId { get; set; }
    public Ad Ad { get; set; }
    public DateTime DateAdded { get; set; }
}