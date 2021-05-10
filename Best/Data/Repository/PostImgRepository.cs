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
        public async Task CreateAvatar(Post post)
        {
            var uploadPath = $"/Users/{post.BestUser.Id}/Campaigns/{post.Campaign.Id}/Posts/{post.Id}";
            post.Img = await _dropbox.AddAvatarImg(uploadPath,post.ImgFile);
            bestContent.Post.Add(post);
        }
        public async Task UpdateAvatar(Post post)
        {
            var uploadPath = $"/Users/{post.BestUser.Id}/Campaigns/{post.Campaign.Id}/Posts/{post.Id}";
            post.Img = await _dropbox.AddAvatarImg(uploadPath, post.ImgFile);

            bestContent.Post.Update(post);
            await bestContent.SaveChangesAsync();
        }
        public async Task DeleteAvatar(Post post)
        {
            var uploadPath = $"/Users/{post.BestUser.Id}/Campaigns/{post.Campaign.Id}/Posts/{post.Id}";
            post.Img = await _dropbox.DeleteAvatarImg(uploadPath);
            bestContent.Post.Update(post);
        }
        public async Task DeleteImg(PostImg postImg)
        {
            await _dropbox.DeleteFolder(postImg.Url);
            bestContent.PostImg.Remove(postImg);
            await bestContent.SaveChangesAsync();
        }
        public async Task CreateImgs(Post post)
        {
            var uploadPath = $"/Users/{post.BestUser.Id}/Campaigns/{post.Campaign.Id}/Posts/{post.Id}";
            List<Img> carousel = await _dropbox.AddImgs(uploadPath, post.ImgsFile);
            post.Carousel = carousel.Select( img => new PostImg() { Alt = img.Alt, Url = img.Url, Post = post}).ToList();
            bestContent.Post.Add(post);
        }
        public async Task UpdateImgs(Post post)
        {
            var uploadPath = $"/Users/{post.BestUser.Id}/Campaigns/{post.Campaign.Id}/Posts/{post.Id}";
            List<Img> imgs = await _dropbox.AddImgs(uploadPath, post.ImgsFile);
            var carousel = post.Carousel.ToList();
            carousel.AddRange(imgs.Select(img => new PostImg() { Alt = img.Alt, Url = img.Url, Post = post }).ToList());
            post.Carousel = carousel;
            bestContent.Post.Update(post);
            await bestContent.SaveChangesAsync();
        }
        public async Task DeleteImgs(Post post)
        {
            var imgs = GetImgsByPostId(post.Id).ToList();
            foreach (var img in imgs)
            {
                await DeleteImg(img);
            }
            await _dropbox.DeleteFolder($"/Users/{post.BestUser.Id}/Campaigns/{post.Campaign.Id}/Posts/{post.Id}");
        }
    }
}
