using Ads.Models;
using Ads.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Ads.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IUnitOfWork _unitOfWork;

    public ChatHub(IUnitOfWork unitOfWork, ILogger<ChatHub> logger)
    {
        _unitOfWork = unitOfWork;
    }

    private int? GetCurrentUserId()
    {
        var claim = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (int.TryParse(claim, out var userId))
            return userId;
        return null;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            await Clients.Caller.SendAsync("Error", "Unauthorized");
            Context.Abort();
            return;
        }
        
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }

    public async Task JoinToChat(int chatId)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            throw new HubException("Unauthorized");
        }

        var chat = await _unitOfWork.Chats.GetByIdAsync(chatId);
        if (chat == null)
        {
            throw new HubException("Chat not found");
        }

        if (chat.BuyerId != userId && chat.SellerId != userId)
        {
            throw new HubException("Forbidden");
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
    }

    public async Task LeaveChat(int chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
    }

    public async Task SendMessage(int chatId, string text)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            throw new HubException("Unauthorized");

        var chat = await _unitOfWork.Chats.GetByIdAsync(chatId);
        if (chat == null)
            throw new HubException("Chat not found");

        if (chat.BuyerId != userId && chat.SellerId != userId)
            throw new HubException("Forbidden");
        
        var newMessage = new Message
        {
            ChatId = chatId,
            SenderId = userId.Value,
            Text = text,
            SentAt = DateTime.UtcNow
        };
        await _unitOfWork.Messages.AddAsync(newMessage);
        await _unitOfWork.SaveChangesAsync();

        await Clients.Group(chatId.ToString())
            .SendAsync("ReceiveMessage", chatId, userId.Value, text, newMessage.SentAt);
    }
}