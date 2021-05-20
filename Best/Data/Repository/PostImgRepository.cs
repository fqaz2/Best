using Best.Data.Interfaces;
using Best.Data.Models;
using Best.Data.Models.Img;
using Dropbox.Api;
using Dropbox.Api.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Repository
{
    public class PostImgRepository : IPostImg
    {

        private readonly BestContent bestContent;
        private readonly IDropbox _dropbox;
        public PostImgRepository(BestContent bestContent, IDropbox dropbox)
        {
            this.bestContent = bestContent;
            _dropbox = dropbox;
        }
        public IEnumerable<PostImg> GetImgs => bestContent.PostImg.Include(p => p.Post);
        public IEnumerable<PostImg> GetImgsByPostId(string post_id) => GetImgs.Where(p => p.Post.Id == post_id);
        public PostImg GetImgById(string img_id) => GetImgs.FirstOrDefault(i => i.Id == img_id);
        public async Task AddAvatar(Post post)
        {
            var uploadPath = $"/Users/{post.BestUserId}/Campaigns/{post.CampaignId}/Posts/{post.Id}";
            post.Img = await _dropbox.AddAvatarImg(uploadPath,post.ImgFile);
            bestContent.Post.Update(post);
            await bestContent.SaveChangesAsync();
        }
        public async Task DeleteAvatar(Post post)
        {
            var uploadPath = $"/Users/{post.BestUserId}/Campaigns/{post.CampaignId}/Posts/{post.Id}";
            post.Img = await _dropbox.DeleteAvatarImg(uploadPath);
            bestContent.Post.Update(post);
            await bestContent.SaveChangesAsync();
        }
        public async Task AddImgs(Post post)
        {
            var uploadPath = $"/Users/{post.BestUserId}/Campaigns/{post.CampaignId}/Posts/{post.Id}";
            List<Img> imgs = await _dropbox.AddImgs(uploadPath, post.ImgsFile);
            List<PostImg> carousel = new List<PostImg>();
            if (bestContent.PostImg.Where(p => p.PostId == post.Id).Any())
                carousel = bestContent.PostImg.Include(p => p.Post).ToList();
            carousel.AddRange(imgs.Select(img => new PostImg() { Alt = img.Alt, Url = img.Url, Post = post }).ToList());
            post.Carousel = carousel;
            bestContent.Post.Update(post);
            await bestContent.SaveChangesAsync();
        }
        public async Task DeleteImg(PostImg postImg)
        {
            await _dropbox.DeleteFolder(postImg.Url);
            bestContent.PostImg.Remove(postImg);
            await bestContent.SaveChangesAsync();
        }
        
    }
}
