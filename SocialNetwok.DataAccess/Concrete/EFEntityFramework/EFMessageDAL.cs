using SocialNetwok.Core.DataAccess.EntityFramework;
using SocialNetwok.DataAccess.Abstraction;
using SocialNetwok.Entities.Data;
using SocialNetwok.Entities.Entities;

namespace SocialNetwok.DataAccess.Concrete.EFEntityFramework;

public class EFMessageDAL : EFEntityRepositoryBase<Message,SocialNetworkDbContext>, IMessageDAL
{
	public EFMessageDAL(SocialNetworkDbContext context) : base(context) { }
}
