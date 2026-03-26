namespace Ads.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    
    public int? ParentId { get; set; }
    public Category? Parent { get; set; }
    
    public List<Category> SubCategories { get; set; } = new List<Category>();
    
    public List<Ad> Ads { get; set; } = new List<Ad>();
}