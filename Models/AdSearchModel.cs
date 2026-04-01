namespace Ads.Models;

public class AdSearchModel
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int Price { get; set; }
    public int CategoryId { get; set; }
    public string City { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string House { get; set; } = null!;
}