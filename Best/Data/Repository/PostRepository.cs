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
        public IEnumerable<Post> GetPosts => bestContent.Post.Include(imgs => imgs.Carousel).Include(c => c.Campaing).Include(u => u.BestUser);
        public Post GetPostById(string post_id) => GetPosts.FirstOrDefault(p => p.Id == post_id);
        public IEnumerable<Post> GetPostsByCampaingId(string campaing_Id) => GetPosts.Where(p => p.Campaing.Id == campaing_Id);
        public IEnumerable<Post> GetPostsByUserId(string user_Id) => GetPosts.Where(p => p.Campaing.BestUser.Id == user_Id);
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
            post.Campaing = await bestContent.Campaing.FirstOrDefaultAsync(c => c.Id == post.Campaing.Id);
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
        public async Task<int> DeletePostsByCampaingId(string campaing_Id)
        {
            var posts = GetPostsByCampaingId(campaing_Id).ToList();
            int result = 0;
            foreach (var post in posts)
            {
                result += await Delete(post);
            }
            return result;
        }
    }
}
