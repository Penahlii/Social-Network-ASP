using SocialNetwok.Core.DataAccess;
using SocialNetwok.Core.DataAccess.EntityFramework;
using SocialNetwok.Entities.Data;
using SocialNetwok.Entities.Entities;

namespace SocialNetwok.DataAccess.Abstraction;

public interface IFriendDAL : IEntityRepository<Friend>
{
}
