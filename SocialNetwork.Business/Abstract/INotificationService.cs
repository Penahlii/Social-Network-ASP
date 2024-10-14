using SocialNetwok.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Business.Abstract;

public interface INotificationService
{
	public Task AddNotificationAsync(string senderId, string receiverId, string notificationText);
	public Task RemoveNotificationAsync(int notificationId);
	public Task RemovingAllNotificationsOfUserAsync(string userId);
}
