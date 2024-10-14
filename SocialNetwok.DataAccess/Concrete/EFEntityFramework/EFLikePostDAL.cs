using SocialNetwok.Core.DataAccess.EntityFramework;
using SocialNetwok.DataAccess.Abstraction;
using SocialNetwok.Entities.Data;
using SocialNetwok.Entities.Entities;

namespace SocialNetwok.DataAccess.Concrete.EFEntityFramework;

public class EFLikePostDAL : EFEntityRepositoryBase<LikePost, SocialNetworkDbContext>, ILikePostDAL
{
	public EFLikePostDAL(SocialNetworkDbContext context) : base(context) { }
}
