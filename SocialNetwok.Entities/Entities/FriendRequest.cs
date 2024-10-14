using SocialNetwok.Core.Abstraction;
using SocialNetwok.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwok.Entities.Entities;

public class FriendRequest : IEntity
{
	public int Id { get; set; }
	public string? SenderId { get; set; }  
	public string? ReceiverId { get; set; }  
	public virtual CustomIdentityUser? Sender { get; set; }
	public virtual CustomIdentityUser? Receiver { get; set; }
	public DateTime RequestDate { get; set; }  
	public DateTime AcceptedDate { get; set; }  
	public StatusOfRequest Status { get; set; } 
}
