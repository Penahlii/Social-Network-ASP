using SocialNetwok.Core.DataAccess.EntityFramework;
using SocialNetwok.DataAccess.Abstraction;
using SocialNetwok.Entities.Data;
using SocialNetwok.Entities.Entities;

namespace SocialNetwok.DataAccess.Concrete.EFEntityFramework;

public class EFFriendDAL : EFEntityRepositoryBase<Friend, SocialNetworkDbContext>, IFriendDAL
{
	public EFFriendDAL(SocialNetworkDbContext context) : base(context) { }
}
