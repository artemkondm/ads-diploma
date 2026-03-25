namespace Ads.DTO.Ads;

public record AdCreateRequest(
    string Title,
    string Description,
    int Price,
    string Region, 
    string City,
    string Street, 
    string House
);

public class AdUpdateRequest
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int Price { get; set; }
}