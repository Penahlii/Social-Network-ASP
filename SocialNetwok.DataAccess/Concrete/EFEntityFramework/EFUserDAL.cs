using SocialNetwok.Core.DataAccess.EntityFramework;
using SocialNetwok.DataAccess.Abstraction;
using SocialNetwok.Entities.Data;
using SocialNetwok.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwok.DataAccess.Concrete.EFEntityFramework;

public class EFUserDAL : EFEntityRepositoryBase<CustomIdentityUser, SocialNetworkDbContext>, IUserDAL
{
    public EFUserDAL(SocialNetworkDbContext context) : base(context) { }
}
