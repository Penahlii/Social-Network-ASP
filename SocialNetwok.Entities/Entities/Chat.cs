using SocialNetwok.Core.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwok.Entities.Entities;

public class Chat : IEntity
{
	public int Id { get; set; }
	public virtual CustomIdentityUser? User1 { get; set; }
	public virtual CustomIdentityUser? User2 { get; set; }
	public virtual ICollection<Message>? Messages { get; set; }
	public string? User1Id { get; set; }
	public string? User2Id { get; set; }
	public Chat()
	{
		Messages = new List<Message>();
	}
}
