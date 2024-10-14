using SocialNetwok.DataAccess.Abstraction;
using SocialNetwok.DataAccess.Concrete.EFEntityFramework;
using SocialNetwok.Entities.Entities;
using SocialNetwork.Business.Abstract;

namespace SocialNetwork.Business.Concrete;

public class ChatService : IChatService
{
	private readonly IChatDAL _chatDAL;

	public ChatService(IChatDAL chatDAL)
	{
		_chatDAL = chatDAL;
	}

	public async Task AddChatAsync(string user1Id, string user2Id)
	{
		var allChats = await _chatDAL.GetList();
		var chatExsists = allChats.Exists(c => c.User1Id == user1Id && c.User2Id == user2Id || c.User1Id == user2Id && c.User2Id == user1Id);
		if (!chatExsists)
		{
			var chat = new Chat
			{
				User1Id = user1Id,
				User2Id = user2Id,
			};
			await _chatDAL.Add(chat);
		}
	}

	public async Task DeleteChatAsync(string user1Id, string user2Id)
	{
		var chat = await GetChatAsync(user1Id, user2Id);
		await _chatDAL.Delete(chat);
	}

	public async Task<Chat> GetChatAsync(string user1Id, string user2Id)
	{
		await AddChatAsync(user1Id, user2Id);
		var chats = await _chatDAL.GetList();
		var chat = chats.FirstOrDefault(c => c.User1Id == user1Id && c.User2Id == user2Id || c.User1Id == user2Id && c.User2Id == user1Id);
		return chat;
	}

	public async Task<List<Chat>> GetChatsByReceiverOrSenderIdAsync(string id)
	{
		var chats = await _chatDAL.GetList();
		var conditionalChats = chats.Where(c => c.User1Id == id || c.User2Id == id).ToList();
		return conditionalChats;
	}
}
