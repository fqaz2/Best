using Best.Data.Interfaces;
using Best.Data.Models;
using Best.Data.Models.Like;
using Microsoft.AspNetCore.Mvc;
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
        public IEnumerable<Post> GetPostsByUserId(string user_Id) => bestContent.Post.Include(imgs => imgs.Carousel).Include(c => c.Campaign).Where(p => p.Campaign.BestUserId == user_Id);
        public Post GetPostByIdForUser(string user_id, string post_id) => GetPostsByUserId(user_id).FirstOrDefault(p => p.Id == post_id);
        //CRUD
        public async Task Create(Post post)
        {
            post.CreateData = DateTime.Now;
            bestContent.Post.Add(post);
            await bestContent.SaveChangesAsync();

            await _dropbox.CreateFolder($"/Users/{post.BestUserId}/Campaigns/{post.CampaignId}/Posts/{post.Id}");

            if (post.ImgFile != null) await _postImg.AddAvatar(post);
            if (post.ImgsFile != null) await _postImg.AddImgs(post);
        }
        public async Task Update(Post post)
        {
            post.CreateData = DateTime.Now;

            bestContent.Post.Update(post);
            await bestContent.SaveChangesAsync();

            if (post.ImgFile != null) await _postImg.AddAvatar(post);
            if (post.ImgsFile != null) await _postImg.AddImgs(post);
        }
        public async Task Delete(string postId)
        {
            Post post = GetPostById(postId);
            //delete when create cascade delete
            //start
            bestContent.PostImg.RemoveRange(post.Carousel);
            bestContent.PostLike.RemoveRange(post.Likes);
            //end
            bestContent.Remove(post);
            await bestContent.SaveChangesAsync();

            await _dropbox.DeleteFolder($"/Users/{post.BestUserId}/Campaigns/{post.CampaignId}/Posts/{post.Id}");
        }
        public async Task<bool> LikePost(string postId, string userId)
        {
            var result = bestContent.PostLike.Where(l => l.Post.Id == postId && l.BestUser.Id == userId).ToList();
            if (result.Count != 0)
            {
                bestContent.PostLike.RemoveRange(result);
                await bestContent.SaveChangesAsync();
                return false;
            }
            bestContent.Add(new PostLike() { PostId = postId, BestUserId = userId});
            await bestContent.SaveChangesAsync();
            return true;
        }
    }
}
