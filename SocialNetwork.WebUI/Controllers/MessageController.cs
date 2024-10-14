using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetwok.DataAccess.Abstraction;
using SocialNetwok.DataAccess.Concrete.EFEntityFramework;
using SocialNetwork.Business.Abstract;

namespace SocialNetwork.WebUI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
	private readonly IMessageService _messageService;
	private readonly IMessageDAL _messageDAL;

	public MessageController(IMessageDAL messageDAL, IMessageService messageService)
	{
		_messageDAL = messageDAL;
		_messageService = messageService;
	}

	[HttpGet("AddMessage")]
	public async Task<IActionResult> AddMessage(int chatId, string senderId, string receiverId, string messageText)
	{
		await _messageService.AddMessageAsync(chatId, senderId, receiverId, messageText);
		return Ok();
	}

	[HttpGet("SetMessagesReaden")]
	public async Task<IActionResult> SetMessagesReaden(string senderId, string receiverId)
	{
		var messages = await _messageDAL.GetList();
		foreach (var message in messages)
		{
			if (message.SenderId == senderId && message.ReceiverId == receiverId)
			{
				message.IsRead = true;
				await _messageDAL.Update(message);
			}
		}
		return Ok();
	}
}
