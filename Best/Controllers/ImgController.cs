using Best.Data.Interfaces;
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
        private readonly ICampaignImg _CampaignImg;
        private readonly ICampaigns _Campaigns;
        public ImgController(IPostImg postImg, IPosts posts, ICampaignImg CampaignImg, ICampaigns Campaigns)
        {
            _postImg = postImg;
            _posts = posts;
            _CampaignImg = CampaignImg;
            _Campaigns = Campaigns;
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
            return RedirectToRoute(new { controller = "Posts", action = "Edit", id = post.Id });
        }
        public async Task<IActionResult> IndexCampaignImg(Campaign Campaign)
        {
            Campaign = _Campaigns.GetCampaignById(Campaign.Id);
            return View(Campaign);
        }
        public async Task<IActionResult> DeleteCampaignImg(string id)
        {
            CampaignImg CampaignImg = _CampaignImg.GetImgById(id);
            await _CampaignImg.DeleteImg(CampaignImg);
            return RedirectToRoute(new { controller = "Campaigns", action = "Edit", id = CampaignImg.Campaign.Id });//controller = "Post",action = "Edit",id = postImg.Post.Id});
        }
        public async Task<IActionResult> AddCampaignImgs(Campaign Campaign)
        {
            var newcampaign = _Campaigns.GetCampaignById(Campaign.Id);
            newcampaign.ImgsFile = Campaign.ImgsFile;
            if (newcampaign.ImgsFile != null) await _CampaignImg.UpdateImgs(newcampaign);
            return RedirectToRoute(new { controller = "Campaigns", action = "Edit", id = Campaign.Id });
        }

    }
}
