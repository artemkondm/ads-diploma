using System.ComponentModel.DataAnnotations;
using Ads.Enums;

namespace Ads.Models;

public class Review
{
    public int Id { get; set; }
    public User Reviewer { get; set; }
    public int ReviewerId { get; set; }
    public User Seller { get; set; }
    public int SellerId { get; set; }
    public Ad Ad { get; set; }
    public int AdId { get; set; }
    public Chat? Chat { get; set; }
    public int ChatId { get; set; }
    public string Comment { get; set; }
    public DateTime Date { get; set; }
    [Range(1, 5)]
    public int Rating { get; set; }

    public ReviewStatus Status { get; set; } = ReviewStatus.OnModeration;
}