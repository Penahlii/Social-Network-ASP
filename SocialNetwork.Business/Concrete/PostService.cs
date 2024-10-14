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

public class PostService : IPostService
{
    private readonly IPostDAL _postDAL;

    public PostService(IPostDAL postDAL)
    {
        _postDAL = postDAL;
    }

    public async Task AddPostAsync(Post post)
    {
        await _postDAL.Add(post);
    }

    public async Task DeletePostAsync(int id)
    {
        var post = await _postDAL.Get(p => p.Id == id);
        await _postDAL.Delete(post);
    }

    public async Task<List<Post>> GetAllPostsAsync()
    {
        return await _postDAL.GetList();
    }

    public async Task<Post> GetPostAsync(Expression<Func<Post, bool>> filter)
    {
        return await _postDAL.Get(filter);
    }

    public async Task UpdatePostAsync(Post post)
    {
        await _postDAL.Update(post);
    }
}
