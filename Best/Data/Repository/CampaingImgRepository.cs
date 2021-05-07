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
    public class CampaingImgRepository : ICampaingImg
    {

        private readonly BestContent bestContent;
        private readonly IDropbox _dropbox;
        public CampaingImgRepository(BestContent bestContent, IDropbox dropbox)
        {
            this.bestContent = bestContent;
            _dropbox = dropbox;
        }
        public IEnumerable<CampaingImg> GetImgs => bestContent.CampaingImg.Include(c => c.Campaing);
        public IEnumerable<CampaingImg> GetImgsByCampaingId(string campaign_id) => GetImgs.Where(p => p.Campaing.Id == campaign_id);
        public CampaingImg GetImgById(string img_id) => GetImgs.FirstOrDefault(i => i.Id == img_id);
        public async Task CreateAvatar(Campaing campaing)
        {
            var uploadPath = $"/Campaigns/{campaing.Id}";
            campaing.Img = await _dropbox.CreateAvatarImg(uploadPath, campaing.ImgFile);
            bestContent.Campaing.Add(campaing);
        }
        public async Task UpdateAvatar(Campaing campaing)
        {
            var uploadPath = $"/Campaigns/{campaing.Id}";
            campaing.Img = await _dropbox.UpdateAvatarImg(uploadPath, campaing.ImgFile);

            bestContent.Campaing.Update(campaing);
            await bestContent.SaveChangesAsync();
        }
        public async Task DeleteAvatar(Campaing campaing)
        {
            var uploadPath = $"/Campaigns/{campaing.Id}";
            campaing.Img = await _dropbox.DeleteAvatarImg(uploadPath);
            bestContent.Campaing.Update(campaing);
        }
        public async Task DeleteImg(CampaingImg campaingImg)
        {
            await _dropbox.DeleteFolder(campaingImg.Url);
            bestContent.CampaingImg.Remove(campaingImg);
            await bestContent.SaveChangesAsync();
        }
        public async Task CreateImgs(Campaing campaing)
        {
            var uploadPath = $"/Campaigns/{campaing.Id}";
            List<Img> carousel = await _dropbox.AddImgs(uploadPath, campaing.ImgsFile);
            campaing.Carousel = carousel.Select( img => new CampaingImg() { Alt = img.Alt, Url = img.Url, Campaing = campaing }).ToList();
            bestContent.Campaing.Add(campaing);
        }
        public async Task UpdateImgs(Campaing campaing)
        {
            var uploadPath = $"/Campaigns/{campaing.Id}";
            List<Img> imgs = await _dropbox.AddImgs(uploadPath, campaing.ImgsFile);
            var carousel = campaing.Carousel.ToList();
            carousel.AddRange(imgs.Select(img => new CampaingImg() { Alt = img.Alt, Url = img.Url, Campaing = campaing }).ToList());
            campaing.Carousel = carousel;
            bestContent.Campaing.Update(campaing);
            await bestContent.SaveChangesAsync();
        }
        public async Task DeleteImgs(Campaing campaing)
        {
            var imgs = GetImgsByCampaingId(campaing.Id).ToList();
            foreach (var img in imgs)
            {
                await DeleteImg(img);
            }
            await _dropbox.DeleteFolder($"/Campaigns/{campaing.Id}");
        }
    }
}
