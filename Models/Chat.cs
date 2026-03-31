namespace Ads.Models;

public class Chat
{
    public int Id { get; set; }
    public int AdId { get; set; }
    public Ad Ad { get; set; } = null!;
    
    public int BuyerId { get; set; }
    public User Buyer { get; set; } = null!;
    public int SellerId { get; set; }
    public User Seller { get; set; } = null!;
    
    public List<Message> Messages { get; set; } = new List<Message>();
}