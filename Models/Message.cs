using System.Text.Json.Serialization;

namespace Ads.Models;

public class Message
{
    public int Id { get; set; }
    public int ChatId { get; set; }
    [JsonIgnore]
    public Chat Chat { get; set; }
    
    public int SenderId { get; set; }
    public string Text { get; set; } = null!;
    public DateTime SentAt { get; set; }
}