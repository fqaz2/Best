using Best.Data.Models;
using Best.Data.Models.Img;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Interfaces
{
    public interface ICampaignImg
    {
        public IEnumerable<CampaignImg> GetImgs { get; }
        public IEnumerable<CampaignImg> GetImgsByCampaignId(string Campaign_id);
        public CampaignImg GetImgById(string img_id);
        public Task CreateAvatar(Campaign Campaign);
        public Task UpdateAvatar(Campaign Campaign);
        public Task DeleteAvatar(Campaign Campaign);
        public Task CreateImgs(Campaign Campaign);
        public Task UpdateImgs(Campaign Campaign);
        public Task DeleteImgs(Campaign Campaign);
        Task DeleteImg(CampaignImg CampaignImg);
    }
}
