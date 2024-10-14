using SocialNetwok.Core.Abstraction;

namespace SocialNetwok.Entities.Entities;

public class LikeComment : IEntity
{
	public int Id { get; set; }
	public string? UserId { get; set; }
	public int CommentId { get; set; }
	public virtual CustomIdentityUser? User { get; set; }
	public virtual Comment? Comment { get; set; }
	public DateTime LikedAt { get; set; }
}
