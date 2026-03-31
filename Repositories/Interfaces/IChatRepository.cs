using System.Linq.Expressions;
using Ads.Models;

namespace Ads.Repositories.Interfaces;

public interface IChatRepository
{
    Task AddAsync(Chat chat);
    Task<Chat?> GetByIdAsync(int chatId);
    Task<List<Chat>> GetUserChatsAsync(int userId);
    Task<IEnumerable<Chat>> FindAsync(Expression<Func<Chat, bool>> predicate);
}