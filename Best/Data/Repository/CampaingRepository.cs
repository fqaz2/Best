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
        public IEnumerable<Campaing> GetCampaings => bestContent.Campaing.Include(t => t.Topic);

        public Campaing GetCampaingById(string campaing_id) => bestContent.Campaing.FirstOrDefault(c => c.Id == campaing_id);

        public IEnumerable<Campaing> GetCampaingsByUserId(string user_id) => GetCampaings.Where(c => c.BestUserId == user_id);
        public Campaing GetCampaingByIdForUser(string user_id, string campaing_id) => GetCampaingsByUserId(user_id).FirstOrDefault(c => c.Id == campaing_id);
        //CRUD
        public async Task<bool> Create(Campaing campaing)
        {
            bestContent.Campaing.Add(campaing);
            var result = await bestContent.SaveChangesAsync();
            if (result > 0)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> Update(Campaing campaing)
        {
            bestContent.Campaing.Update(campaing);
            var result = await bestContent.SaveChangesAsync();
            if (result > 0)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> Delete(Campaing campaing)
        {
            await _posts.DeletePosts(_posts.GetPostsByCampaingId(campaing.Id));

            bestContent.Campaing.Remove(campaing);
            var result = await bestContent.SaveChangesAsync();
            if (result > 0)
            {
                return true;
            }
            return false;
        }
    }
}
