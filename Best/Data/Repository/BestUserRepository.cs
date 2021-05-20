using Best.Areas.Identity.Data;
using Best.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
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
        private readonly IDropbox _dropbox;
        private readonly SignInManager<BestUser> _signInManager;
        public BestUserRepository(BestContent bestContent, ICampaigns Campaigns, IDropbox dropbox, SignInManager<BestUser> signInManager)
        {
            this.bestContent = bestContent;
            _dropbox = dropbox;
            _signInManager = signInManager;
        }
        public IEnumerable<BestUser> GetUsers => bestContent.BestUser.Include(cr => cr.CampaignRating).Include(pl => pl.PostLike).Include(imgs => imgs.Carousel).Include(c => c.Campaigns).Include(p => p.Posts);
        public BestUser GetUserById(string user_id) => GetUsers.FirstOrDefault(u => u.Id == user_id);
        public async Task Delete(String bestUserId)
        {
            var bestUser = GetUserById(bestUserId);
            await _dropbox.DeleteFolder($"/Users/{bestUser.Id}");
            bestContent.BestUser.Remove(bestUser);
            await bestContent.SaveChangesAsync();

            await _signInManager.SignOutAsync();
        }
    }
}
