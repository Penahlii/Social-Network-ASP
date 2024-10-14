using SocialNetwok.Core.DataAccess.EntityFramework;
using SocialNetwok.DataAccess.Abstraction;
using SocialNetwok.Entities.Data;
using SocialNetwok.Entities.Entities;

namespace SocialNetwok.DataAccess.Concrete.EFEntityFramework;

public class EFChatDAL : EFEntityRepositoryBase<Chat, SocialNetworkDbContext>, IChatDAL
{
	public EFChatDAL(SocialNetworkDbContext context) : base(context) { }
}
