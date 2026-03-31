using Ads.Models;

namespace Ads.Repositories.Interfaces;

public interface IMessageRepository
{
    Task AddAsync(Message message);
    Task<List<Message>> FindMessagesAsync(int chatId);
}