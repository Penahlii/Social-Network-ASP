using SocialNetwok.Core.DataAccess.EntityFramework;
using SocialNetwok.DataAccess.Abstraction;
using SocialNetwok.Entities.Data;
using SocialNetwok.Entities.Entities;

namespace SocialNetwok.DataAccess.Concrete.EFEntityFramework;

public class EFCommentDAL : EFEntityRepositoryBase<Comment, SocialNetworkDbContext>, ICommentDAL
{
	public EFCommentDAL(SocialNetworkDbContext context) : base(context) { }
}
