using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SocialNetwok.DataAccess.Abstraction;
using SocialNetwok.DataAccess.Concrete.EFEntityFramework;
using SocialNetwok.Entities.Entities;
using SocialNetwok.Entities.Enums;
using SocialNetwork.Business.Abstract;

namespace SocialNetwork.Business.Concrete;

public class FriendRequestService : IFriendRequestService
{
	private readonly IFriendRequestDAL _friendRequestDAL;
	private readonly IHttpContextAccessor _context;
	private readonly IFriendService _friendService;
	private readonly UserManager<CustomIdentityUser> _userManager;

	public FriendRequestService(IFriendRequestDAL friendRequestDAL, IHttpContextAccessor context, IFriendService friendService, UserManager<CustomIdentityUser> userManager)
	{
		_friendRequestDAL = friendRequestDAL;
		_context = context;
		_friendService = friendService;
		_userManager = userManager;
	}

	public async Task AcceptFriendRequestAsync(string senderId)
	{
		var friendRequests = await _friendRequestDAL.GetList();

		var currentUser = await _userManager.GetUserAsync(_context.HttpContext.User);

		var updatedRequest = friendRequests.FirstOrDefault(fr => fr.SenderId == senderId && fr.ReceiverId == currentUser.Id);
		updatedRequest.Status = StatusOfRequest.Accepted;
		updatedRequest.AcceptedDate = DateTime.Now;
		await _friendRequestDAL.Update(updatedRequest);

		var newRequest = new FriendRequest
		{
			SenderId = currentUser.Id,
			ReceiverId = senderId,
			Status = StatusOfRequest.Accepted,
			AcceptedDate = DateTime.Now,
		};
		await _friendRequestDAL.Add(newRequest);

		await _friendService.AddFriendAsync(senderId);
	}

	public async Task CancelFriendRequestAsync(string receiverId)
	{
		var currentUser = await _userManager.GetUserAsync(_context.HttpContext.User);
		var friendRequests = await _friendRequestDAL.GetList();
		var friendRequest = friendRequests.FirstOrDefault(fr => fr.SenderId == currentUser.Id && fr.ReceiverId == receiverId);
		await _friendRequestDAL.Delete(friendRequest);
	}

	public async Task<List<FriendRequest>> GetAllFriendRequestsAsync()
	{
		var friendRequests = await _friendRequestDAL.GetList();
		return friendRequests;
	}

	public async Task<List<FriendRequest>> GetAllSentFriendRequestsOfCurrentUserAsync()
	{
		var currentUser = await _userManager.GetUserAsync(_context.HttpContext.User);
		var friendRequests = await _friendRequestDAL.GetList();
		return friendRequests.Where(fr => fr.ReceiverId == currentUser.Id && fr.Status == StatusOfRequest.Pending).ToList();
	}

	public async Task<List<FriendRequest>> GetFriendRequestsCurrentUserAsync(string key = "")
	{
		var currentUser = await _userManager.GetUserAsync(_context.HttpContext.User);
		var friendRequests = await _friendRequestDAL.GetList();
		if (key != "") return friendRequests.Where(fr => fr.ReceiverId == currentUser.Id || fr.SenderId == currentUser.Id && fr.Receiver.UserName.Contains(key) || fr.Sender.UserName.Contains(key)).ToList();
		return friendRequests.Where(fr => fr.ReceiverId == currentUser.Id && fr.Status == StatusOfRequest.Pending || fr.SenderId == currentUser.Id && fr.Status == StatusOfRequest.Pending).ToList();
	}

	public async Task RejectFriendRequestAsync(string senderId)
	{
		var currentUser = await _userManager.GetUserAsync(_context.HttpContext.User);
		var friendRequests = await _friendRequestDAL.GetList();
		var friendRequest = friendRequests.FirstOrDefault(fr => fr.SenderId == senderId && fr.ReceiverId == currentUser.Id);
		await _friendRequestDAL.Delete(friendRequest);
	}

	public async Task SendFriendRequestAsync(string receiverId)
	{
		var currentUser = await _userManager.GetUserAsync(_context.HttpContext.User);
		var newFriendRequest = new FriendRequest
		{
			SenderId = currentUser.Id,
			ReceiverId = receiverId,
			RequestDate = DateTime.Now,
			Status = StatusOfRequest.Pending,
		};
		await _friendRequestDAL.Add(newFriendRequest);
	}
}
