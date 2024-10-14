using SocialNetwok.Entities.Entities;

namespace SocialNetwork.Business.Abstract;

public interface IChatService
{
	public Task AddChatAsync(string user1Id, string user2Id);
	public Task<Chat> GetChatAsync(string user1Id, string user2Id);
	public Task DeleteChatAsync(string user1Id, string user2Id);
	public Task<List<Chat>> GetChatsByReceiverOrSenderIdAsync(string id);
}
