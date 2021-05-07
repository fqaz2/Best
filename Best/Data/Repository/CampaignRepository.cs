﻿using Best.Areas.Identity.Data;
using Best.Data.Interfaces;
using Best.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Repository
{
    public class CampaignRepository : ICampaigns
    {
        private readonly BestContent bestContent;
        private readonly IPosts _posts;
        private readonly IDropbox _dropbox;
        private readonly ICampaignImg _CampaignImg;
        private readonly ITopics _topics;
        private readonly UserManager<BestUser> _userManager;
        public CampaignRepository(BestContent bestContent, IPosts posts, IDropbox dropbox, ICampaignImg CampaignImg, ITopics topics, UserManager<BestUser> userManager)
        {
            this.bestContent = bestContent;
            _posts = posts;
            _dropbox = dropbox;
            _CampaignImg = CampaignImg;
        }
        public IEnumerable<Campaign> GetCampaigns => bestContent.Campaign.Include(t => t.Topic).Include(p => p.Posts).Include(imgs => imgs.Carousel).Include(u => u.BestUser);

        public Campaign GetCampaignById(string Campaign_id) => GetCampaigns.FirstOrDefault(c => c.Id == Campaign_id);

        public IEnumerable<Campaign> GetCampaignsByUserId(string user_id) => GetCampaigns.Where(c => c.BestUser.Id == user_id);
        public Campaign GetCampaignByIdForUser(string user_id, string Campaign_id) => GetCampaignsByUserId(user_id).FirstOrDefault(c => c.Id == Campaign_id);
        //CRUD
        public async Task<int> Create(Campaign Campaign)
        {
            bestContent.Campaign.Add(Campaign);
            await _dropbox.CreateFolder($"/Campaigns/{Campaign.Id}");
            if (Campaign.ImgFile != null) await _CampaignImg.CreateAvatar(Campaign);
            if (Campaign.ImgsFile != null) await _CampaignImg.CreateImgs(Campaign);
            return await bestContent.SaveChangesAsync();
        }
        public async Task Update(Campaign Campaign)
        {
            Campaign.Topic = await bestContent.Topic.FirstOrDefaultAsync(t => t.Id == Campaign.Topic.Id); ;
            Campaign.BestUser = await bestContent.BestUser.FirstOrDefaultAsync(u => u.Id == Campaign.BestUser.Id);
            bestContent.Campaign.Update(Campaign);
            await bestContent.SaveChangesAsync();

            if (Campaign.ImgFile != null) await _CampaignImg.UpdateAvatar(Campaign);
            if (Campaign.ImgsFile != null) await _CampaignImg.UpdateImgs(Campaign);
        }
        public async Task<int> Delete(Campaign Campaign)
        {
            await _posts.DeletePostsByCampaignId(Campaign.Id);
            if (Campaign.Img != null || Campaign.Carousel != null) await _CampaignImg.DeleteImgs(Campaign);
            Campaign = GetCampaignById(Campaign.Id);
            bestContent.Campaign.Remove(Campaign);
            return await bestContent.SaveChangesAsync();
        }
        public async Task<int> DeleteCampaignsByUserId(string user_id)
        {
            var Campaigns = GetCampaignsByUserId(user_id).ToList();
            int result = 0;
            foreach (var Campaign in Campaigns)
            {
                result += await Delete(Campaign);
            }
            return result;
        }
    }
}