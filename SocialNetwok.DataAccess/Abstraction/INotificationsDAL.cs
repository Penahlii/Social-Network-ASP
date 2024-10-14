using SocialNetwok.Core.DataAccess;
using SocialNetwok.Entities.Entities;

namespace SocialNetwok.DataAccess.Abstraction;

public interface INotificationsDAL : IEntityRepository<Notification>
{
}
