using SocialNetwok.Core.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwok.Entities.Entities;

public class Notification : IEntity
{
	public int Id { get; set; }
	public virtual CustomIdentityUser? Receiver { get; set; }
	public virtual CustomIdentityUser? Sender { get; set; }
	public string? ReceiverId { get; set; }
	public string? SenderId { get; set; }
	public DateTime SentAt { get; set; }
	public string? NotificationText { get; set; }
}
