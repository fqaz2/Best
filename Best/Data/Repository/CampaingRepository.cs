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
        public CampaingRepository(BestContent bestContent)
        {
            this.bestContent = bestContent;
        }
        public IEnumerable<Campaing> GetCampaings => bestContent.Campaing.Include(t => t.Topic);

        public Campaing GetCampaingById(string campaing_id) => bestContent.Campaing.FirstOrDefault(c => c.Id == campaing_id);
        //CRUD
        public async Task<bool> Add(Campaing Campaing)
        {
            try
            {
                await bestContent.Campaing.AddAsync(Campaing);
                await bestContent.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
