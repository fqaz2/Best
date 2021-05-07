using Best.Data.Models.Img;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Interfaces
{
    public interface IDropbox
    {
        Task<string> getImgByUrl(string Path);
        Task<string> CreateAvatarImg(string uploadPath, IFormFile file);
        Task<string> UpdateAvatarImg(string uploadPath, IFormFile file);
        Task<string> DeleteAvatarImg(string uploadPath);
        Task CreateFolder(string uploadPath);
        Task DeleteFolder(string uploadPath);
        Task<List<Img>> AddImgs(string uploadPath, IEnumerable<IFormFile> files);

    }
}
