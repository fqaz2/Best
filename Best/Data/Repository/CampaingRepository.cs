using Best.Areas.Identity.Data;
using Best.Data.Interfaces;
using Best.Data.Models;
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
        public CampaingRepository(BestContent bestContent, IPosts posts)
        {
            this.bestContent = bestContent;
            _posts = posts;
        }
        public IEnumerable<Campaing> GetCampaings => bestContent.Campaing.Include(t => t.Topic).Include(p => p.Posts).Include(imgs => imgs.Carusel).Include(u => u.BestUser);

        public Campaing GetCampaingById(string campaing_id) => GetCampaings.FirstOrDefault(c => c.Id == campaing_id);

        public IEnumerable<Campaing> GetCampaingsByUserId(string user_id) => GetCampaings.Where(c => c.BestUser.Id == user_id);
        public Campaing GetCampaingByIdForUser(string user_id, string campaing_id) => GetCampaingsByUserId(user_id).FirstOrDefault(c => c.Id == campaing_id);
        //CRUD
        public async Task<int> Create(Campaing campaing)
        {
            bestContent.Campaing.Add(campaing);
            return await bestContent.SaveChangesAsync();
        }
        public async Task<int> Update(Campaing campaing)
        {
            bestContent.Campaing.Update(campaing);
            return await bestContent.SaveChangesAsync();
        }
        public async Task<int> Delete(Campaing campaing)
        {
            await _posts.DeletePostsByCampaingId(campaing.Id);
            bestContent.CampaingImg.RemoveRange(campaing.Carusel);

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
