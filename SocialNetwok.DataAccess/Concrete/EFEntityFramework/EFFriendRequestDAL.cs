using SocialNetwok.Core.DataAccess.EntityFramework;
using SocialNetwok.DataAccess.Abstraction;
using SocialNetwok.Entities.Data;
using SocialNetwok.Entities.Entities;

namespace SocialNetwok.DataAccess.Concrete.EFEntityFramework;

public class EFFriendRequestDAL : EFEntityRepositoryBase<FriendRequest, SocialNetworkDbContext>, IFriendRequestDAL
{
	public EFFriendRequestDAL(SocialNetworkDbContext context) : base(context) { }
}
