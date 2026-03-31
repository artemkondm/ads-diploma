using Ads.DTO.Chat;
using Ads.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ads.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ChatController(IChatService chatService) : ControllerBase
{
    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] SendMessageRequest request)
    {
        var senderId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
        
        if (string.IsNullOrWhiteSpace(request.Text))
            return BadRequest("Сообщение не может быть пустым");
        try
        {
            await chatService.SendMessageAsync(request.ChatId, senderId, request.Text);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetMyChats()
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
        var chats = await chatService.GetUserChatsWithLastMessageAsync(userId);
        return Ok(chats);
    }

    [HttpGet("{chatId}/messages")]
    public async Task<IActionResult> GetMessages(int chatId)
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
        try
        {
            var messages = await chatService.GetChatMessagesAsync(chatId, userId);
            return Ok(messages);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpPost("startChat")]
    public async Task<IActionResult> StartChat([FromBody] StartChatRequest request)
    {
        var buyerId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
        try
        {
            int chatId = await chatService.SendFirstMessageAsync(request.AdId, buyerId, request.Text);
            return Ok(new { ChatId = chatId });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}