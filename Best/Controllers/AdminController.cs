using Best.Areas.Identity.Data;
using Best.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<BestUser> _userManager;
        private readonly SignInManager<BestUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICampaigns _campaigns;
        private readonly IPosts _posts;
        public AdminController( UserManager<BestUser> userManager, SignInManager<BestUser> signInManager, RoleManager<IdentityRole> roleManager, ICampaigns campaigns, IPosts posts)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;

            _campaigns = campaigns;
            _posts = posts;
        }
        public async Task<bool> IsAdmin()
        {
            var isAdmin = await _userManager.IsInRoleAsync(await _userManager.GetUserAsync(User), "Administrator");
            var isSignIn = _signInManager.IsSignedIn(User);
            if (!isSignIn || !isAdmin)
            {
                return false;
            }
            return true;
        }
        public async Task<IActionResult> Index()
        {
            if (!await IsAdmin()) return NotFound();
            return View();
        }
        public async Task<IActionResult> Campaigns()
        {
            if (!await IsAdmin()) return NotFound();
            return View(_campaigns.GetCampaigns.ToList());
        }
        public async Task<IActionResult> Posts()
        {
            if (!await IsAdmin()) return NotFound();
            return View(_posts.GetPosts.ToList());
        }
    }
}
