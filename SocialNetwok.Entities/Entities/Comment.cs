using SocialNetwok.Core.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwok.Entities.Entities;

public class Comment : IEntity
{
	public int Id { get; set; }
	public string? CommentText { get; set; }
	public DateTime CreatedAt { get; set; }
	public virtual CustomIdentityUser? User { get; set; }
	public virtual Post? Post { get; set; }
	public virtual ICollection<LikeComment>? Likes { get; set; }
	public string? UserId { get; set; }
	public int PostId { get; set; }
	public Comment()
	{
		Likes = new List<LikeComment>();
	}
}
