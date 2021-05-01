using Best.Areas.Identity.Data;
using Best.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Repository
{
    public class BestUserRepository : IBestUsers
    {
        private readonly BestContent bestContent;
        private readonly ICampaings _campaings;
        public BestUserRepository(BestContent bestContent, ICampaings campaings)
        {
            this.bestContent = bestContent;
            _campaings = campaings;
        }
        public IEnumerable<BestUser> GetUsers => bestContent.BestUser.Include(c => c.Campaings).Include(p => p.Posts);
        public BestUser GetUserById(string user_id) => GetUsers.FirstOrDefault(u => u.Id == user_id);
        public async Task<int> Delete(BestUser bestUser)
        {
            await _campaings.DeleteCampaingsByUserId(bestUser.Id);

            bestUser = GetUserById(bestUser.Id);
            bestContent.BestUser.Remove(bestUser);
            return await bestContent.SaveChangesAsync();
        }
    }
}
