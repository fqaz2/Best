using Best.Areas.Identity.Data;
using Best.Data;
using Best.Data.Interfaces;
using Best.Data.Models;
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
        private readonly ICampaings _campaings;
        private readonly BestContent _context;
        public UsersController(UserManager<BestUser> userManager, SignInManager<BestUser> signInManager, RoleManager<IdentityRole> roleManager, BestContent context, ICampaings campaings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _campaings = campaings;
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
            BestUser bestUser = await _userManager.FindByIdAsync(id);
            return View(bestUser);
        }

        // POST: UsersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(BestUser bestUser)
        {
            try
            {
                bestUser = await _userManager.FindByIdAsync(bestUser.Id);
                bestUser.Campaings = _campaings.GetCampaingsByUserId(bestUser.Id);
                await _campaings.DeleteCampaingsByUser(bestUser);//need to create Repository for BestUser
                await _userManager.DeleteAsync(bestUser);

                if (bestUser.Id == _userManager.GetUserId(User))
                {
                    await _signInManager.SignOutAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View();
            }
        }
    }
}
