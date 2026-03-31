namespace Ads.DTO.Chat;

public class UserChatResponse
{
    public int ChatId { get; set; }
    public int AdId { get; set; }
    public string AdTitle { get; set; } = null!;
    public string InterlocutorName { get; set; } = null!;
    public string LastMessageText { get; set; } = null!;
    public DateTime LastMessageAt { get; set; }
}