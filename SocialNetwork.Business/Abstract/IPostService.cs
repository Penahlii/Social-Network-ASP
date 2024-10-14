using SocialNetwok.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Business.Abstract;

public interface IPostService
{
    Task AddPostAsync(Post post);
    Task DeletePostAsync(int id);
    Task<Post> GetPostAsync(Expression<Func<Post, bool>> filter);
    Task<List<Post>> GetAllPostsAsync();
    Task UpdatePostAsync(Post post);
}
