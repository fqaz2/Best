﻿using Best.Data.Models;
using Best.Data.Models.Img;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Interfaces.IImg
{
    public interface ICampaignImg
    {
        public IEnumerable<CampaignImg> GetImgs { get; }
        public IEnumerable<CampaignImg> GetImgsByCampaignId(string Campaign_id);
        public CampaignImg GetImgById(string img_id);
        public Task AddAvatar(Campaign Campaign);
        public Task DeleteAvatar(Campaign Campaign);
        public Task AddImgs(Campaign Campaign);
        Task DeleteImg(CampaignImg campaignImg);
    }
}
