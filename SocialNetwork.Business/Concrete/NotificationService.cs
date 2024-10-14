using SocialNetwok.DataAccess.Abstraction;
using SocialNetwok.DataAccess.Concrete.EFEntityFramework;
using SocialNetwok.Entities.Entities;
using SocialNetwork.Business.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Business.Concrete;

public class NotificationService : INotificationService
{
	private readonly INotificationsDAL _notificationsDAL;

	public NotificationService(INotificationsDAL notificationsDAL)
	{
		_notificationsDAL = notificationsDAL;
	}

	public async Task AddNotificationAsync(string senderId, string receiverId, string notificationText)
	{
		var notification = new Notification
		{
			SenderId = senderId,
			ReceiverId = receiverId,
			NotificationText = notificationText,
			SentAt = DateTime.UtcNow,
		};
		await _notificationsDAL.Add(notification);
	}

	public async Task RemoveNotificationAsync(int notificationId)
	{
		var notifications = await _notificationsDAL.GetList();
		var notification = notifications.FirstOrDefault(n => n.Id == notificationId);
		await _notificationsDAL.Delete(notification);
	}

	public async Task RemovingAllNotificationsOfUserAsync(string userId)
	{
		var notifications = await _notificationsDAL.GetList();
		for (int i = 0; i < notifications.Count; i++)
		{
			if (notifications[i].ReceiverId == userId) await _notificationsDAL.Delete(notifications[i]);
		}
	}
}
