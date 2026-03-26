namespace Ads.Models;

public class Ad
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; }
    public int Price { get; set; }
    public DateTime DateCreated { get; set; }
    public int UserId { get; set; } 
    public User User { get; set; } = null!;
    public Location Location { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}