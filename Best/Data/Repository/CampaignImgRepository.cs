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
        public async Task CreateAvatar(Campaign Campaign)
        {
            var uploadPath = $"/Users/{Campaign.BestUser.Id}/Campaigns/{Campaign.Id}";
            Campaign.Img = await _dropbox.AddAvatarImg(uploadPath, Campaign.ImgFile);
            bestContent.Campaign.Add(Campaign);
        }
        public async Task UpdateAvatar(Campaign Campaign)
        {
            var uploadPath = $"/Users/{Campaign.BestUser.Id}/Campaigns/{Campaign.Id}";
            Campaign.Img = await _dropbox.AddAvatarImg(uploadPath, Campaign.ImgFile);

            bestContent.Campaign.Update(Campaign);
            await bestContent.SaveChangesAsync();
        }
        public async Task DeleteAvatar(Campaign Campaign)
        {
            var uploadPath = $"/Users/{Campaign.BestUser.Id}/Campaigns/{Campaign.Id}";
            Campaign.Img = await _dropbox.DeleteAvatarImg(uploadPath);
            bestContent.Campaign.Update(Campaign);
        }
        public async Task DeleteImg(CampaignImg CampaignImg)
        {
            await _dropbox.DeleteFolder(CampaignImg.Url);
            bestContent.CampaignImg.Remove(CampaignImg);
            await bestContent.SaveChangesAsync();
        }
        public async Task CreateImgs(Campaign Campaign)
        {
            var uploadPath = $"/Users/{Campaign.BestUser.Id}/Campaigns/{Campaign.Id}";
            List<Img> carousel = await _dropbox.AddImgs(uploadPath, Campaign.ImgsFile);
            Campaign.Carousel = carousel.Select( img => new CampaignImg() { Alt = img.Alt, Url = img.Url, Campaign = Campaign }).ToList();
            bestContent.Campaign.Add(Campaign);
        }
        public async Task UpdateImgs(Campaign Campaign)
        {
            var uploadPath = $"/Users/{Campaign.BestUser.Id}/Campaigns/{Campaign.Id}";
            List<Img> imgs = await _dropbox.AddImgs(uploadPath, Campaign.ImgsFile);
            var carousel = Campaign.Carousel.ToList();
            carousel.AddRange(imgs.Select(img => new CampaignImg() { Alt = img.Alt, Url = img.Url, Campaign = Campaign }).ToList());
            Campaign.Carousel = carousel;
            bestContent.Campaign.Update(Campaign);
            await bestContent.SaveChangesAsync();
        }
        public async Task DeleteImgs(Campaign Campaign)
        {
            var imgs = GetImgsByCampaignId(Campaign.Id).ToList();
            foreach (var img in imgs)
            {
                await DeleteImg(img);
            }
            await _dropbox.DeleteFolder($"/Users/{Campaign.BestUser.Id}/Campaigns/{Campaign.Id}");
        }
    }
}
