using Best.Data.Interfaces;
using Best.Data.Interfaces.IImg;
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

namespace Best.Data.Repository.ImgRepository
{
    public class CampaignImgRepository : ICampaignImg
    {

        private readonly BestContent bestContent;
        private readonly IDropbox _dropbox;
        public CampaignImgRepository(BestContent bestContent, IDropbox dropbox)
        {
            this.bestContent = bestContent;
            _dropbox = dropbox;
        }
        public IEnumerable<CampaignImg> GetImgs => bestContent.CampaignImg.Include(c => c.Campaign);
        public IEnumerable<CampaignImg> GetImgsByCampaignId(string campaign_id) => GetImgs.Where(p => p.Campaign.Id == campaign_id);
        public CampaignImg GetImgById(string img_id) => GetImgs.FirstOrDefault(i => i.Id == img_id);
        public async Task AddAvatar(Campaign campaign)
        {
            var uploadPath = $"/Users/{campaign.BestUserId}/Campaigns/{campaign.Id}";
            campaign.Img = await _dropbox.AddAvatarImg(uploadPath, campaign.ImgFile);
            bestContent.Campaign.Update(campaign);
            await bestContent.SaveChangesAsync();
        }
        public async Task DeleteAvatar(Campaign campaign)
        {
            var uploadPath = $"/Users/{campaign.BestUserId}/Campaigns/{campaign.Id}";
            campaign.Img = await _dropbox.DeleteAvatarImg(uploadPath);
            bestContent.Campaign.Update(campaign);
            await bestContent.SaveChangesAsync();
        }
        public async Task AddImgs(Campaign campaign)
        {
            var uploadPath = $"/Users/{campaign.BestUserId}/Campaigns/{campaign.Id}";
            List<Img> imgs = await _dropbox.AddImgs(uploadPath, campaign.ImgsFile);
            List<CampaignImg> carousel = new List<CampaignImg>();
            if (bestContent.CampaignImg.Where(c => c.CampaignId == campaign.Id).Any())
                carousel = bestContent.CampaignImg.Include(c => c.Campaign).ToList();
            carousel.AddRange(imgs.Select(img => new CampaignImg() { Alt = img.Alt, Url = img.Url, Campaign = campaign }).ToList());
            campaign.Carousel = carousel;
            bestContent.Campaign.Update(campaign);
            await bestContent.SaveChangesAsync();
        }
        public async Task DeleteImg(CampaignImg campaignImg)
        {
            await _dropbox.DeleteFolder(campaignImg.Url);
            bestContent.CampaignImg.Remove(campaignImg);
            await bestContent.SaveChangesAsync();
        }
    }
}
