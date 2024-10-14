using SocialNetwok.Entities.Entities;

namespace SocialNetwork.Business.Abstract;

public interface IFriendRequestService
{
	public Task<List<FriendRequest>> GetFriendRequestsCurrentUserAsync(string key = "");
	public Task<List<FriendRequest>> GetAllFriendRequestsAsync();
	public Task SendFriendRequestAsync(string receiverId);
	public Task RejectFriendRequestAsync(string senderId);
	public Task AcceptFriendRequestAsync(string senderId);
	public Task CancelFriendRequestAsync(string receiverId);
	public Task<List<FriendRequest>> GetAllSentFriendRequestsOfCurrentUserAsync();
}
