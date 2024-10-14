using SocialNetwok.DataAccess.Abstraction;
using SocialNetwok.DataAccess.Concrete.EFEntityFramework;
using SocialNetwok.Entities.Entities;
using SocialNetwork.Business.Abstract;

namespace SocialNetwork.Business.Concrete;

public class MessageService : IMessageService
{
	private readonly IMessageDAL _messageDAL;

	public MessageService(IMessageDAL messageDAL)
	{
		_messageDAL = messageDAL;
	}

	public async Task AddMessageAsync(int chatId, string senderId, string receiverId, string messageText)
	{
		var msg = new Message
		{
			ChatId = chatId,
			SenderId = senderId,
			ReceiverId = receiverId,
			MessageText = messageText,
			IsRead = false,
			SentAt = DateTime.Now,
		};
		await _messageDAL.Add(msg);
	}
}
