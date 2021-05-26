using Best.Areas.Identity.Data;
using Best.Data.Interfaces;
using Best.Data.Interfaces.IImg;
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
        public IEnumerable<Campaign> GetCampaigns => bestContent.Campaign.Include(t => t.Topic).Include(p => p.Posts).Include(imgs => imgs.Carousel).Include(u => u.BestUser).Include(r => r.Ratings);

        public Campaign GetCampaignById(string Campaign_id) => GetCampaigns.FirstOrDefault(c => c.Id == Campaign_id);

        public IEnumerable<Campaign> GetCampaignsByUserId(string user_id) => GetCampaigns.Where(c => c.BestUser.Id == user_id);
        public Campaign GetCampaignByIdForUser(string user_id, string Campaign_id) => GetCampaignsByUserId(user_id).FirstOrDefault(c => c.Id == Campaign_id);
        //CRUD
        public async Task Create(Campaign Campaign)
        {
            Campaign.CreateData = DateTime.Now;
            bestContent.Campaign.Add(Campaign);
            await bestContent.SaveChangesAsync();

            await _dropbox.CreateFolder($"/Users/{Campaign.BestUserId}/Campaigns/{Campaign.Id}");
            
            if (Campaign.ImgFile != null) await _CampaignImg.AddAvatar(Campaign);
            if (Campaign.ImgsFile != null) await _CampaignImg.AddImgs(Campaign);
        }
        public async Task Update(Campaign Campaign)
        {
            Campaign.CreateData = DateTime.Now;
            bestContent.Campaign.Update(Campaign);
            await bestContent.SaveChangesAsync();

            if (Campaign.ImgFile != null) await _CampaignImg.AddAvatar(Campaign);
            if (Campaign.ImgsFile != null) await _CampaignImg.AddImgs(Campaign);
        }
        public async Task Delete(string campaignId)
        {
            Campaign campaign = GetCampaignById(campaignId);
            //delete when create cascade delete
            //start
            campaign.Posts.ToList()
                .ForEach(p =>
                {
                    p = _posts.GetPostById(p.Id);
                    bestContent.PostImg.RemoveRange(p.Carousel);
                    bestContent.PostLike.RemoveRange(p.Likes);
                    bestContent.PostComments.RemoveRange(p.Comments);
                });
            bestContent.Post.RemoveRange(campaign.Posts);
            bestContent.CampaignRating.RemoveRange(campaign.Ratings);
            //end
            bestContent.Remove(campaign);
            await bestContent.SaveChangesAsync();

            await _dropbox.DeleteFolder($"/Users/{campaign.BestUserId}/Campaigns/{campaign.Id}");
        }
        public async Task<double> Rating(string Campaign_id)
        {
            var ratings = bestContent.CampaignRating.Where(c => c.CampaignId == Campaign_id);
            if (!ratings.Any())
                return 0;
            return await ratings.Select(r => r.Rating).AverageAsync();
        }
    }
}
