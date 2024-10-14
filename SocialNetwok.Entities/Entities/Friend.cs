using SocialNetwok.Core.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwok.Entities.Entities;

public class Friend : IEntity
{
	public int Id { get; set; }
	public string? YourFriendId { get; set; }
	public string? OwnId { get; set; }
	public virtual CustomIdentityUser? YourFriend { get; set; }
	public virtual CustomIdentityUser? Own { get; set; }
}
