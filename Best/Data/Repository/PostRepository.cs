using Best.Data.Interfaces;
using Best.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Repository
{
    public class PostRepository : IPosts
    {
        private readonly BestContent bestContent;
        public PostRepository(BestContent bestContent)
        {
            this.bestContent = bestContent;
        }
        public IEnumerable<Post> GetPosts => bestContent.Post;

        public Post GetPostById(string post_id) => bestContent.Post.FirstOrDefault(p => p.Id == post_id);

        public IEnumerable<Post> GetPostsByCampaingId(string campaing_Id) => bestContent.Post.Where(p => p.Campaing.Id == campaing_Id);
    }
}
