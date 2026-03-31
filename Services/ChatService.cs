using Ads.DTO.Chat;
using Ads.Hubs;
using Ads.Models;
using Ads.Repositories.Interfaces;
using Ads.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Ads.Services;

public class ChatService(IUnitOfWork unitOfWork, IHubContext<ChatHub> hubContext) : IChatService
{
    public async Task SendMessageAsync(int chatId, int senderId, string text)
    {
        var message = new Message { ChatId = chatId, SenderId = senderId, Text = text, SentAt = DateTime.UtcNow };
        
        await unitOfWork.Messages.AddAsync(message);
        await unitOfWork.SaveChangesAsync();
        
        await hubContext.Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", senderId, text, message.SentAt);
    }

    public async Task<IEnumerable<Message>> GetChatMessagesAsync(int chatId, int userId)
    {
        var chat = await unitOfWork.Chats.GetByIdAsync(chatId);
        if (chat == null || (chat.BuyerId != userId && chat.SellerId != userId))
        {
            throw new UnauthorizedAccessException("Нет доступа к чату");
        }

        return await unitOfWork.Messages.FindMessagesAsync(chatId);
    }

    public async Task<IEnumerable<Chat>> GetUserChatsAsync(int userId)
    {
        return await unitOfWork.Chats.GetUserChatsAsync(userId);
    }

    public async Task<int> SendFirstMessageAsync(int adId, int buyerId, string text)
    {
        var existingChats = await unitOfWork.Chats.FindAsync(chat => chat.BuyerId == buyerId && chat.AdId == adId);
        var chat = existingChats.FirstOrDefault();

        if (chat == null)
        {
            var ad = await unitOfWork.Ads.GetByIdAsync(adId);
            if (ad == null) throw new Exception("Объявление не найдено");
            if (ad.UserId == buyerId) throw new Exception("Вы не можете создать чат с самим собой");
            
            chat = new Chat { AdId = adId, BuyerId = buyerId, SellerId = ad.UserId };
            
            await unitOfWork.Chats.AddAsync(chat);
            await unitOfWork.SaveChangesAsync();
        }
        await SendMessageAsync(chat.Id, buyerId, text);
        return chat.Id;
    }

    public async Task<IEnumerable<UserChatResponse>> GetUserChatsWithLastMessageAsync(int userId)
    {
        var chats = await unitOfWork.Chats.FindAsync(chat => chat.SellerId == userId || chat.BuyerId == userId);

        var chatList = chats.Select(chat => new UserChatResponse
            {
                ChatId = chat.Id,
                AdId = chat.AdId,
                AdTitle = chat.Ad.Title,
                InterlocutorName = chat.BuyerId == userId ? chat.Seller.Name : chat.Buyer.Name,
                LastMessageText = chat.Messages.OrderByDescending(m => m.SentAt).FirstOrDefault()?.Text ??
                                  "Нет сообщений",
                LastMessageAt = chat.Messages.OrderByDescending(m => m.SentAt).FirstOrDefault()?.SentAt ??
                                DateTime.MinValue,
            })
            .OrderByDescending(c => c.LastMessageAt)
            .ToList();
        return chatList;
    }
}