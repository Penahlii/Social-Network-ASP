using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialNetwok.DataAccess.Abstraction;
using SocialNetwok.DataAccess.Concrete.EFEntityFramework;
using SocialNetwok.Entities.Entities;
using SocialNetwork.Business.Abstract;

namespace SocialNetwork.Business.Concrete;

public class FriendService : IFriendService
{
	private readonly IFriendDAL _friendDAL;
	private readonly IHttpContextAccessor _context;
	private readonly UserManager<CustomIdentityUser> _userManager;
	private readonly IFriendRequestDAL _friendRequestDAL;

	public FriendService(IFriendDAL friendDAL, IHttpContextAccessor context, UserManager<CustomIdentityUser> userManager, IFriendRequestDAL friendRequestDAL)
	{
		_friendDAL = friendDAL;
		_context = context;
		_userManager = userManager;
		_friendRequestDAL = friendRequestDAL;
	}

	public async Task AddFriendAsync(string friendId)
	{
		var currentUser = await _userManager.GetUserAsync(_context.HttpContext.User);
		var newFriend = new Friend
		{
			OwnId = currentUser.Id,
			YourFriendId = friendId,
		};
		await _friendDAL.Add(newFriend);
	}

	public async Task<List<Friend>> GetFriendsOfCurrentUserAsync(string key = "")
	{
		var currentUser = await _userManager.GetUserAsync(_context.HttpContext.User);
		var friends = await _friendDAL.GetList();
		if (key != "") return friends.Where(f => f.YourFriendId == currentUser.Id || f.OwnId == currentUser.Id && f.YourFriend.UserName.Contains(key) || f.Own.UserName.Contains(key)).ToList();
		return friends.Where(f => f.YourFriendId == currentUser.Id || f.OwnId == currentUser.Id).ToList();
	}

	public async Task RemoveFriendAsync(string friendId)
	{
		var currentUser = await _userManager.GetUserAsync(_context.HttpContext.User);
		var friends = await _friendDAL.GetList();
		var friendRow = friends.FirstOrDefault(f => f.YourFriendId == friendId && f.OwnId == currentUser.Id || f.OwnId == friendId && f.YourFriendId == currentUser.Id);

		var friendRequests = await _friendRequestDAL.GetList();
		var friendRequestRow = friendRequests.FirstOrDefault(fr => fr.SenderId == friendId && fr.ReceiverId == currentUser.Id || fr.ReceiverId == friendId && fr.SenderId == currentUser.Id);
		var friendRequest2Row = friendRequests.FirstOrDefault(fr => fr.SenderId == friendId && fr.ReceiverId == currentUser.Id || fr.ReceiverId == friendId && fr.SenderId == currentUser.Id);

		await _friendDAL.Delete(friendRow);

		await _friendRequestDAL.Delete(friendRequestRow);
		await _friendRequestDAL.Delete(friendRequest2Row);
	}

	public async Task<List<CustomIdentityUser>> GetOtherPeopleAsync(string key = "")
	{
		var currentUser = await _userManager.GetUserAsync(_context.HttpContext.User);
		var users = await _userManager.Users.ToListAsync();

		var currentUserFriends = currentUser.Friends.Select(f => f.YourFriendId).ToList();
		var currentUserFriends2 = currentUser.Friends.Select(f => f.OwnId).ToList();

		var allRequests = await _friendRequestDAL.GetList();
		var sentFriendRequests = allRequests.Where(fr => fr.SenderId == currentUser.Id).Select(fr => fr.ReceiverId).ToList();
		var receivedFriendRequests = allRequests.Where(fr => fr.ReceiverId == currentUser.Id).Select(fr => fr.SenderId).ToList();

		var otherUsers = users.Where(u =>
			u.Id != currentUser.Id &&  
			!currentUserFriends.Contains(u.Id) && !currentUserFriends2.Contains(u.Id) &&  
			!sentFriendRequests.Contains(u.Id) &&  
			!receivedFriendRequests.Contains(u.Id) 
		);

		if (!string.IsNullOrEmpty(key))
		{
			otherUsers = otherUsers.Where(u => u.UserName.Contains(key));
		}

		return otherUsers.ToList();
	}
}
