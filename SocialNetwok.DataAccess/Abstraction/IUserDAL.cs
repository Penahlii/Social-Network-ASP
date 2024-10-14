using SocialNetwok.Core.DataAccess;
using SocialNetwok.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwok.DataAccess.Abstraction;

public interface IUserDAL : IEntityRepository<CustomIdentityUser>
{
}
