using SocialNetwok.Core.DataAccess.EntityFramework;
using SocialNetwok.DataAccess.Abstraction;
using SocialNetwok.Entities.Data;
using SocialNetwok.Entities.Entities;

namespace SocialNetwok.DataAccess.Concrete.EFEntityFramework;

public class EFLikeCommentDAL : EFEntityRepositoryBase<LikeComment, SocialNetworkDbContext>, ILikeCommentDAL
{
	public EFLikeCommentDAL(SocialNetworkDbContext context) : base(context) { }
}
