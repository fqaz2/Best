using Best.Data.Models;
using Best.Data.Models.Img;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Interfaces
{
    public interface IImg
    {
        public Task CreateImgForPost(Post post);
        public Task UpdateImgForPost(Post post); 
        public Task DeleteImgsForPost(Post post);
    }
}
