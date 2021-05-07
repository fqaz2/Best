﻿using Best.Data.Interfaces;
using Best.Data.Models;
using Best.Data.Models.Img;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Controllers
{
    public class ImgController : Controller
    {
        private readonly IPostImg _postImg;
        private readonly IPosts _posts;
        public ImgController(IPostImg postImg, IPosts posts)
        {
            _postImg = postImg;
            _posts = posts;
        }
        public async Task<IActionResult> IndexPostImg(Post post)
        {
            post = _posts.GetPostById(post.Id);
            return View(post);
        }
        public async Task<IActionResult> DeletePostImg(string id)
        {
            PostImg postImg = _postImg.GetImgById(id);
            await _postImg.DeleteImg(postImg);
            return RedirectToRoute(new { controller = "Posts", action = "Edit", id = postImg.Post.Id });//controller = "Post",action = "Edit",id = postImg.Post.Id});
        }
        public async Task<IActionResult> AddPostImgs(Post post)
        {
            var newpost = _posts.GetPostById(post.Id);
            newpost.ImgsFile = post.ImgsFile;
            if (newpost.ImgsFile != null) await _postImg.UpdateImgs(newpost);
            return RedirectToRoute(new {controller = "Posts", action = "Edit", id = post.Id });
        }

    }
}