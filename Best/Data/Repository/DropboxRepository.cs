using Best.Data.Interfaces;
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
    public class DropboxRepository : IDropbox
    {
        private static string token = "tRhgoiHzCdoAAAAAAAAAASnYCm02HrIB84CM2Ew21QAcKleQZByIuc9UgZhcjMcG";
        public async Task<string> getImgByUrl(string Path)
        {
            using (var dbx = new DropboxClient(token))
            {
                var TemporaryLink = await dbx.Files.GetTemporaryLinkAsync(Path);
                return TemporaryLink.Link;
            }
        }
        public async Task<string> AddAvatarImg(string uploadPath, IFormFile file)
        {
            using (var dbx = new DropboxClient(token))
            {
                var PathImg = $"{uploadPath}/Avatar";
                var metadata = await dbx.Files.UploadAsync(PathImg, WriteMode.Overwrite.Instance, body: file.OpenReadStream());

                return metadata.PathLower;
            }
        }
        public async Task<string> DeleteAvatarImg(string uploadPath)
        {
            using (var dbx = new DropboxClient(token))
            {
                var PathImg = $"{uploadPath}/Avatar";
                await dbx.Files.DeleteV2Async(PathImg);

                return null;
            }
        }
        public async Task CreateFolder(string uploadPath)
        {
            using (var dbx = new DropboxClient(token))
            {
                var PathImg = $"{uploadPath}";
                await dbx.Files.CreateFolderV2Async(PathImg, false);
            }
        }
        public async Task DeleteFolder(string uploadPath)
        {
            using (var dbx = new DropboxClient(token))
            {
                var PathImg = $"{uploadPath}";
                await dbx.Files.DeleteV2Async(PathImg);
            }
        }

        public async Task<List<Img>> AddImgs(string uploadPath, IEnumerable<IFormFile> files)
        {
            using (var dbx = new DropboxClient(token))
            {
                var Folderlist = await dbx.Files.ListFolderAsync(uploadPath);
                uploadPath = $"{uploadPath}/Carousel";
                if (!Folderlist.Entries.Where(i=> i.IsFolder).Any(i =>i.Name == "Carousel"))
                {
                    await dbx.Files.CreateFolderV2Async(uploadPath);
                }
                
                List<Img> imgs = new List<Img>();
                foreach (var file in files)
                {
                    var PathImg = $"{uploadPath}/{file.FileName}";
                    var metadata = await dbx.Files.UploadAsync(PathImg, WriteMode.Add.Instance, true, body: file.OpenReadStream());
                    imgs.Add(new Img()
                    {
                        Url = metadata.PathLower,
                        Alt = metadata.Name
                    });
                }

                return imgs;
            }
        }
    }
}
