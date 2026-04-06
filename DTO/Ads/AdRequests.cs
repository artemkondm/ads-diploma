using System.ComponentModel.DataAnnotations;

namespace Ads.DTO.Ads;

public record AdCreateRequest(
    string Title,
    string Description,
    int Price,
    int CategoryId,
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

public class CreateAdRequest
{
    [Required]
    [StringLength(100, MinimumLength = 5)]
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    [Range(0, 10000000)]
    public int Price { get; set; }
    public int CategoryId { get; set; }
    public string Region { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string House { get; set; } = null!;
    [Required(ErrorMessage = "Добавьте хотя бы одно фото")]
    public List<IFormFile> Images { get; set; } = null!;
}