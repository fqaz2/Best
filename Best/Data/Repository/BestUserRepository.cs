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
        //taste side branch
        private readonly BestContent bestContent;
        private readonly IDropbox _dropbox;
        private readonly SignInManager<BestUser> _signInManager;
        private readonly UserManager<BestUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        //delete when create cascade delete
        //start
        private readonly ICampaigns _campaigns;
        private readonly IPosts _posts;
        //end

        public BestUserRepository(BestContent bestContent, ICampaigns Campaigns, IDropbox dropbox, SignInManager<BestUser> signInManager, UserManager<BestUser> userManager, RoleManager<IdentityRole> roleManager, ICampaigns campaigns, IPosts posts)
        {
            this.bestContent = bestContent;
            _dropbox = dropbox;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;

            _campaigns = campaigns;
            _posts = posts;
        }
        public IEnumerable<BestUser> GetUsers => bestContent.BestUser.Include(cr => cr.CampaignRating).Include(pl => pl.PostLike).Include(imgs => imgs.Carousel).Include(c => c.Campaigns).Include(p => p.Posts);
        public BestUser GetUserById(string user_id) => GetUsers.FirstOrDefault(u => u.Id == user_id);
        public async Task Delete(String bestUserId)
        {
            var bestUser = GetUserById(bestUserId);

            //delete when create cascade delete
            //start
            bestUser.Campaigns.ToList().ForEach
                (c => {
                    c = _campaigns.GetCampaignById(c.Id);
                    c.Posts.ToList()
                        .ForEach(p =>
                        {
                            p = _posts.GetPostById(p.Id);
                            bestContent.PostImg.RemoveRange(p.Carousel);
                            bestContent.PostLike.RemoveRange(p.Likes);
                        });
                    bestContent.Post.RemoveRange(c.Posts);
                    bestContent.CampaignRating.RemoveRange(c.Ratings);
            
                    bestContent.Remove(c);
                });
            bestContent.PostLike.RemoveRange(bestUser.PostLike);
            bestContent.CampaignRating.RemoveRange(bestUser.CampaignRating);
            //end

            await _dropbox.DeleteFolder($"/Users/{bestUser.Id}");
            bestContent.BestUser.Remove(bestUser);
            await bestContent.SaveChangesAsync();

            await _signInManager.SignOutAsync();
        }

        public async Task Block(string bestUserId)
        {
            BestUser bestUser = await bestContent.BestUser.SingleAsync(u => u.Id == bestUserId);
            if (bestUser.IsBlock)
            {
                bestUser.IsBlock = false;
                await _userManager.SetLockoutEndDateAsync(bestUser, DateTime.Now);
            }else
            {
                bestUser.IsBlock = true;
                await _userManager.SetLockoutEndDateAsync(bestUser, new DateTime(9999,01,01));
            }
            bestContent.Users.Update(bestUser);
            await bestContent.SaveChangesAsync();
        }

        public async Task AddRole(string bestUserId, string roleId)
        {
            BestUser bestUser = await _userManager.FindByIdAsync(bestUserId);
            IdentityRole identityRole = await _roleManager.FindByIdAsync(roleId);
            await _userManager.AddToRoleAsync(bestUser,identityRole.Name);
        }
    }
}
