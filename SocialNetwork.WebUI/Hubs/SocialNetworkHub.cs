using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SocialNetwok.Entities.Entities;

namespace SocialNetwork.WebUI.Hubs;

public class SocialNetworkHub : Hub
{
	private readonly IHttpContextAccessor _contextAccessor;
	private readonly UserManager<CustomIdentityUser> _userManager;

	public SocialNetworkHub(IHttpContextAccessor contextAccessor, UserManager<CustomIdentityUser> userManager)
	{
		_contextAccessor = contextAccessor;
		_userManager = userManager;
	}

	public override async Task OnConnectedAsync()
	{
		Console.WriteLine($"User connected: {Context.ConnectionId}");
		await Clients.All.SendAsync("UpdateContacts");
	}

	public override async Task OnDisconnectedAsync(Exception? exception)
	{
		var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext.User);
		user.IsOnline = false;
		await _userManager.UpdateAsync(user);
		await Clients.All.SendAsync("UpdateContacts");
	}

	public async Task UpdateContactsForAllUsers()
	{
		await Clients.All.SendAsync("UpdateContacts");
	}
	public async Task UpdateContactsForOtherUsers()
	{
		await Clients.Others.SendAsync("UpdateContacts");
	}

	public async Task UpdateUserMessagesForReceiver(string receiverId, string senderId)
	{
		await Clients.User(receiverId).SendAsync("UpdateAllMessages", senderId);
	}

	public async Task UpdateNotificationsForReceiver(string receiverId)
	{
		await Clients.User(receiverId).SendAsync("UpdateNotificationsForReceiver");
	}

	public async Task UpdateFriendRequestsAndFriendsForUsers()
	{
		await Clients.All.SendAsync("UpdateFriendRequestsAndFriends");
	}
}
