using System.Linq.Expressions;
using Ads.Database;
using Ads.Models;
using Ads.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ads.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly AppDbContext _context;

    public ChatRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Chat?> GetByIdAsync(int chatId)
    {
        return await _context.Chats.FindAsync(chatId);
    }

    public async Task<List<Chat>> GetUserChatsAsync(int userId)
    {
       return await _context.Chats.Where(c => c.BuyerId == userId || c.SellerId == userId)
           .ToListAsync();
    }

    public async Task<IEnumerable<Chat>> FindAsync(Expression<Func<Chat, bool>> predicate)
    {
        return await _context.Chats
            .Where(predicate)
            .Include(c => c.Seller)
            .Include(c => c.Buyer)
            .Include(c => c.Ad)
            .ToListAsync();
    }

    public async Task AddAsync(Chat chat)
    {
        await _context.Chats.AddAsync(chat);
    }
}