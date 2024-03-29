﻿using Best.Data.Models;
using Best.Data.Models.Img;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Interfaces.IImg
{
    public interface IPostImg
    {
        public IEnumerable<PostImg> GetImgs { get; }
        public IEnumerable<PostImg> GetImgsByPostId(string post_id);
        public PostImg GetImgById(string img_id);
        public Task AddAvatar(Post post);
        public Task DeleteAvatar(Post post);
        public Task AddImgs(Post post);
        Task DeleteImg(PostImg postImg);
    }
}
