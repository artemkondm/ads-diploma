using System.ComponentModel.DataAnnotations;

namespace Ads.Models;

public class Location
{
    [Key]
    public int Id { get; set; }
    
    public int AdId { get; set; }
    public Ad Ad { get; set; } = null!;
    public int CityId { get; set; }
    public City City { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string House { get; set; } = null!;
    
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? YandexUri { get; set; }
}