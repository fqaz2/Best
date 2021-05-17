using Best.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Interfaces
{
    public interface IPosts
    {
        IEnumerable<Post> GetPosts { get; }
        IEnumerable<Post> GetPostsByCampaignId(string Campaign_Id);
        IEnumerable<Post> GetPostsByUserId(string user_Id);
        Post GetPostById(string post_id);
        Post GetPostByIdForUser(string user_id, string post_id);
        Task Create(Post post);
        Task Update(Post post);
        Task Delete(string postId);
        Task<bool> LikePost(string postId, string userId);
    }
}
