using SocialNetwok.DataAccess.Abstraction;
using SocialNetwok.Entities.Entities;
using SocialNetwork.Business.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Business.Concrete;

public class UserService : IUserService
{
    private readonly IUserDAL _userDAL;

    public UserService(IUserDAL userDAL)
    {
        _userDAL = userDAL;
    }

    public async Task AddUserAsync(CustomIdentityUser user)
    {
        await _userDAL.Add(user);
    }

    public async Task DeleteUserAsync(string id)
    {
        var user = await _userDAL.Get(u => u.Id == id);
        await _userDAL.Delete(user);
    }

    public async Task<List<CustomIdentityUser>> GetAllUsersAsync()
    {
        return await _userDAL.GetList();
    }

    public async Task<CustomIdentityUser> GetUserAsync(Expression<Func<CustomIdentityUser, bool>> filter)
    {
        return await _userDAL.Get(filter);
    }

    public async Task UpdateUserAsync(CustomIdentityUser user)
    {
        await _userDAL.Update(user);
    }
}
