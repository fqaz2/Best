using Best.Data.Interfaces;
using Best.Data.Models;
using Best.Data.Models.Img;
using Dropbox.Api;
using Dropbox.Api.Files;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Repository
{
    public class ImgRepository : IImg
    {
        private static string token = "sl.AwEJSe9xB8f8md4HUaDw4sRLWIXylc76a7Jcw9bNpUOvAJnnwZ26dbbOIi21olOvIj1IeXBjiz8vM4UoRu50OWBF4pblCRTCW7KjAqAK2sbcJG3qrqRYPyZs-rnZMqlhKYz439HXXo0";
        private readonly BestContent bestContent;
        public ImgRepository(BestContent bestContent)
        {
            this.bestContent = bestContent;
        }
        private async Task<string> CreateImg(string uploadPath, IFormFile file)
        {
            using (var dbx = new DropboxClient(token))
            {
                var uploadPathImg = $"{uploadPath}/avatar";

                await dbx.Files.CreateFolderV2Async(uploadPath);
                await dbx.Files.UploadAsync(uploadPathImg, WriteMode.Overwrite.Instance, body: file.OpenReadStream());

                var TemporaryLink = await dbx.Files.GetTemporaryLinkAsync(uploadPathImg);
                return TemporaryLink.Link;
            }
        }
        private async Task<string> UpdateImg(string uploadPath, IFormFile file)
        {
            using (var dbx = new DropboxClient(token))
            {
                var uploadPathImg = $"{uploadPath}/avatar";
                await dbx.Files.DeleteV2Async(uploadPath);
                await dbx.Files.UploadAsync(uploadPathImg, WriteMode.Overwrite.Instance, body: file.OpenReadStream());
                
                var TemporaryLink = await dbx.Files.GetTemporaryLinkAsync(uploadPathImg);
                return TemporaryLink.Link;
            }
        }
        private async Task DeleteImg(string uploadPath)
        {
            using (var dbx = new DropboxClient(token))
            {
                await dbx.Files.DeleteV2Async(uploadPath);
            }
        }
        public async Task CreateImgForPost(Post post)
        {
            var uploadPath = $"/Posts/{post.Id}";
            post.Img = await CreateImg(uploadPath, post.ImgFile);
            bestContent.Post.Add(post);
        }

        public async Task UpdateImgForPost(Post post)
        {
            var uploadPath = $"/Posts/{post.Id}";
            post.Img = await UpdateImg(uploadPath, post.ImgFile);
            bestContent.Post.Update(post);
        }

        public async Task DeleteImgsForPost(Post post)
        {
            var uploadPath = $"/Posts/{post.Id}";
            await DeleteImg(uploadPath);
        }
    }
}
