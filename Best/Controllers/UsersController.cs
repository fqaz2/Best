using Best.Areas.Identity.Data;
using Best.Data;
using Best.Data.Interfaces;
using Best.Data.Models;
using Best.Data.Models.Сombined;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<BestUser> _userManager;
        private readonly SignInManager<BestUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICampaigns _Campaigns;
        private readonly IPosts _posts;
        private readonly IBestUsers _bestUser;
        private readonly BestContent _context;
        public UsersController(UserManager<BestUser> userManager, SignInManager<BestUser> signInManager, RoleManager<IdentityRole> roleManager, BestContent context, ICampaigns Campaigns, IPosts posts, IBestUsers bestUser)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _Campaigns = Campaigns;
            _posts = posts;
            _bestUser = bestUser;
        }
        public async Task<bool> IsAdmin()
        {
            BestUser bestUser = await _userManager.GetUserAsync(User);
            if (await _userManager.IsInRoleAsync(bestUser, "Administrator"))
            {
                return true;
            }
            return false;
        }
        public ActionResult Index()
        {
            return View(_userManager.Users.ToList());
        }

        // GET: UsersController/Details/5
        public async Task<IActionResult> Details(string id)
        {
            return View(await _userManager.FindByIdAsync(id));
        }

        // GET: UsersController/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (!(_signInManager.IsSignedIn(User) && (_userManager.GetUserId(User) == id || await IsAdmin())))
            {
                return NotFound();
            }
            BestUser bestUser = await _userManager.FindByIdAsync(id);
            return View(bestUser);
        }

        // POST: UsersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(BestUser bestUser)
        {
            await _bestUser.Delete(bestUser.Id);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Block(string id)
        {
            if (!(_signInManager.IsSignedIn(User) && ( _userManager.GetUserId(User) == id || await IsAdmin())))
            {
                return NotFound();
            }
            BestUser bestUser = await _userManager.FindByIdAsync(id);
            return View(bestUser);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Block(BestUser bestUser)
        {
            await _bestUser.Block(bestUser.Id);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> AddRole(string id)
        {
            if (!(_signInManager.IsSignedIn(User) && await IsAdmin()))
            {
                return NotFound();
            }
            return View(new CombUserRole() { BestUser = _bestUser.GetUserById(id) });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(CombUserRole combUserRole)
        {
            await _bestUser.AddRole(combUserRole.BestUser.Id, combUserRole.IdentityRole.Id);
            return RedirectToAction(nameof(Index));
        }
    }
}
