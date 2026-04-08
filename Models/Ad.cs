using Ads.Enums;

namespace Ads.Models;

public class Ad
{
    public int Id { get; set; }
    public AdStatus Status { get; set; }
    public bool IsDeleted { get; set; } = false;
    public string Title { get; set; } = null!;
    public string Description { get; set; }
    public int Price { get; set; }
    public DateTime DateCreated { get; set; }
    public int UserId { get; set; } 
    public User User { get; set; } = null!;
    public Location Location { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public string ThumbnailUrl { get; set; } = null!;
    public List<AdImage> Images { get; set; } = [];
}