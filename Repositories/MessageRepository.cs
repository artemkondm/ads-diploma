using Ads.Database;
using Ads.Models;
using Ads.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ads.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly AppDbContext _context;
    public MessageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Message message)
    {
        await _context.Messages.AddAsync(message);
    }

    public async Task<List<Message>> FindMessagesAsync(int chatId)
    {
        return await _context.Messages
            .Where(m => m.ChatId == chatId)
            .OrderBy(m => m.SentAt)
            .ToListAsync();
    }
}