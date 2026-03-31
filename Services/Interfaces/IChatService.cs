using Ads.DTO.Chat;
using Ads.Models;

namespace Ads.Services.Interfaces;

public interface IChatService
{
    Task SendMessageAsync(int chatId, int senderId, string text);
    Task<IEnumerable<Chat>> GetUserChatsAsync(int userId);
    Task<IEnumerable<Message>> GetChatMessagesAsync(int chatId, int userId);
    Task<int> SendFirstMessageAsync(int adId, int buyerId, string text);
    Task<IEnumerable<UserChatResponse>> GetUserChatsWithLastMessageAsync(int userId);
}