using Best.Data.Interfaces;
using Best.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Repository
{
    public class PostRepository : IPosts
    {
        private readonly BestContent bestContent;
        private readonly IPostImg _postImg;
        private readonly IDropbox _dropbox;
        public PostRepository(BestContent bestContent, IPostImg postImg, IDropbox dropbox)
        {
            this.bestContent = bestContent;
            _postImg = postImg;
            _dropbox = dropbox;
        }
        public IEnumerable<Post> GetPosts => bestContent.Post.Include(imgs => imgs.Carousel).Include(c => c.Campaign).Include(u => u.BestUser).Include(l=> l.Likes);
        public Post GetPostById(string post_id) => GetPosts.FirstOrDefault(p => p.Id == post_id);
        public IEnumerable<Post> GetPostsByCampaignId(string Campaign_Id) => GetPosts.Where(p => p.Campaign.Id == Campaign_Id);
        public IEnumerable<Post> GetPostsByUserId(string user_Id) => GetPosts.Where(p => p.Campaign.BestUser.Id == user_Id);
        public Post GetPostByIdForUser(string user_id, string post_id) => GetPostsByUserId(user_id).FirstOrDefault(p => p.Id == post_id);
        //CRUD
        public async Task<int> Create(Post post)
        {
            bestContent.Post.Add(post);
            await _dropbox.CreateFolder($"/Posts/{post.Id}");
            if (post.ImgFile != null) await _postImg.CreateAvatar(post);
            if (post.ImgsFile != null) await _postImg.CreateImgs(post);
            return await bestContent.SaveChangesAsync();
        }
        public async Task Update(Post post)
        {
            post.Campaign = await bestContent.Campaign.FirstOrDefaultAsync(c => c.Id == post.Campaign.Id);
            post.BestUser = await bestContent.BestUser.FirstOrDefaultAsync(u => u.Id == post.BestUser.Id);
            bestContent.Post.Update(post);
            await bestContent.SaveChangesAsync();

            if (post.ImgFile != null) await _postImg.UpdateAvatar(post);
            if (post.ImgsFile != null) await _postImg.UpdateImgs(post);
        }
        public async Task<int> Delete(Post post)
        {
            post = GetPostById(post.Id);
            if (post.Img != null || post.Carousel != null) await _postImg.DeleteImgs(post);
            bestContent.Post.Remove(post);
            return await bestContent.SaveChangesAsync();
        }
        public async Task<int> DeletePostsByCampaignId(string Campaign_Id)
        {
            var posts = GetPostsByCampaignId(Campaign_Id).ToList();
            int result = 0;
            foreach (var post in posts)
            {
                result += await Delete(post);
            }
            return result;
        }
    }
}
