using Best.Areas.Identity.Data;
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
        public AdminController( UserManager<BestUser> userManager, SignInManager<BestUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
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
    }
}
