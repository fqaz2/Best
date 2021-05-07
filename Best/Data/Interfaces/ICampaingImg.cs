using Best.Data.Models;
using Best.Data.Models.Img;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Interfaces
{
    public interface ICampaingImg
    {
        public IEnumerable<CampaingImg> GetImgs { get; }
        public IEnumerable<CampaingImg> GetImgsByCampaingId(string campaing_id);
        public CampaingImg GetImgById(string img_id);
        public Task CreateAvatar(Campaing campaing);
        public Task UpdateAvatar(Campaing campaing);
        public Task DeleteAvatar(Campaing campaing);
        public Task CreateImgs(Campaing campaing);
        public Task UpdateImgs(Campaing campaing);
        public Task DeleteImgs(Campaing campaing);
        Task DeleteImg(CampaingImg campaingImg);
    }
}
