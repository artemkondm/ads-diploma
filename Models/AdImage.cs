namespace Ads.Models;

public class AdImage
{
    public int Id { get; set; }
    public string Url { get; set; } = null!;
    public string? ThumbnailUrl { get; set; }
    public bool IsMain { get; set; }
    public int AdId { get; set; }
}