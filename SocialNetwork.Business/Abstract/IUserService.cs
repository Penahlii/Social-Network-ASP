using SocialNetwok.Core.Abstraction;
using SocialNetwok.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Business.Abstract;

public interface IUserService
{
    Task AddUserAsync(CustomIdentityUser user);
    Task DeleteUserAsync(string id);
    Task<CustomIdentityUser> GetUserAsync(Expression<Func<CustomIdentityUser, bool>> filter);
    Task<List<CustomIdentityUser>> GetAllUsersAsync();
    Task UpdateUserAsync(CustomIdentityUser user);
}
