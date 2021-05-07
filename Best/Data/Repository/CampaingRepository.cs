using Best.Areas.Identity.Data;
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
    public class CampaingRepository : ICampaings
    {
        private readonly BestContent bestContent;
        private readonly IPosts _posts;
        private readonly IDropbox _dropbox;
        private readonly ICampaingImg _campaingImg;
        private readonly ITopics _topics;
        private readonly UserManager<BestUser> _userManager;
        public CampaingRepository(BestContent bestContent, IPosts posts, IDropbox dropbox, ICampaingImg campaingImg, ITopics topics, UserManager<BestUser> userManager)
        {
            this.bestContent = bestContent;
            _posts = posts;
            _dropbox = dropbox;
            _campaingImg = campaingImg;
        }
        public IEnumerable<Campaing> GetCampaings => bestContent.Campaing.Include(t => t.Topic).Include(p => p.Posts).Include(imgs => imgs.Carousel).Include(u => u.BestUser);

        public Campaing GetCampaingById(string campaing_id) => GetCampaings.FirstOrDefault(c => c.Id == campaing_id);

        public IEnumerable<Campaing> GetCampaingsByUserId(string user_id) => GetCampaings.Where(c => c.BestUser.Id == user_id);
        public Campaing GetCampaingByIdForUser(string user_id, string campaing_id) => GetCampaingsByUserId(user_id).FirstOrDefault(c => c.Id == campaing_id);
        //CRUD
        public async Task<int> Create(Campaing campaing)
        {
            bestContent.Campaing.Add(campaing);
            await _dropbox.CreateFolder($"/Campaigns/{campaing.Id}");
            if (campaing.ImgFile != null) await _campaingImg.CreateAvatar(campaing);
            if (campaing.ImgsFile != null) await _campaingImg.CreateImgs(campaing);
            return await bestContent.SaveChangesAsync();
        }
        public async Task Update(Campaing campaing)
        {
            campaing.Topic = await bestContent.Topic.FirstOrDefaultAsync(t => t.Id == campaing.Topic.Id); ;
            campaing.BestUser = await bestContent.BestUser.FirstOrDefaultAsync(u => u.Id == campaing.BestUser.Id);
            bestContent.Campaing.Update(campaing);
            await bestContent.SaveChangesAsync();

            if (campaing.ImgFile != null) await _campaingImg.UpdateAvatar(campaing);
            if (campaing.ImgsFile != null) await _campaingImg.UpdateImgs(campaing);
        }
        public async Task<int> Delete(Campaing campaing)
        {
            await _posts.DeletePostsByCampaingId(campaing.Id);
            if (campaing.Img != null || campaing.Carousel != null) await _campaingImg.DeleteImgs(campaing);
            campaing = GetCampaingById(campaing.Id);
            bestContent.Campaing.Remove(campaing);
            return await bestContent.SaveChangesAsync();
        }
        public async Task<int> DeleteCampaingsByUserId(string user_id)
        {
            var campaings = GetCampaingsByUserId(user_id).ToList();
            int result = 0;
            foreach (var campaing in campaings)
            {
                result += await Delete(campaing);
            }
            return result;
        }
    }
}
