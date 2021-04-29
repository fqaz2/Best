using Best.Areas.Identity.Data;
using Best.Data;
using Best.Data.Interfaces;
using Best.Data.Models;
using Best.Data.Models.Combined;
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
        public async Task<IActionResult> block(string id)
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
        public async Task<IActionResult> block(BestUser bestUser)
        {
            try
            {
                bestUser = await _userManager.FindByIdAsync(bestUser.Id);
                bestUser.Campaings = _campaings.GetCampaingsByUserId(bestUser.Id);
                if (bestUser.IsBlock)
                {
                    bestUser.IsBlock = false;
                    await _userManager.SetLockoutEndDateAsync(bestUser, DateTime.Now);
                }
                else
                {
                    await _userManager.SetLockoutEndDateAsync(bestUser, new DateTime(9999, 12, 30));
                    if (bestUser.Id == _userManager.GetUserId(User))
                    {
                        await _signInManager.SignOutAsync();
                    }
                }
                await _userManager.UpdateAsync(bestUser);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        public async Task<IActionResult> AddRole(string id)
        {
            if (!(_signInManager.IsSignedIn(User) && await IsAdmin()))
            {
                return NotFound();
            }
            CombUser combUser = new CombUser();
            combUser.BestUser = await _userManager.FindByIdAsync(id);
            combUser.BestUser.Campaings = _campaings.GetCampaingsByUserId(id);
            return View(combUser);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(CombUser combUser)
        {
            BestUser bestUser = await _userManager.FindByIdAsync(combUser.BestUser.Id);
            IdentityRole role = await _roleManager.FindByIdAsync(combUser.IdentityRole.Id);
            await _userManager.AddToRoleAsync( bestUser, role.Name);
            return RedirectToAction(nameof(Index));
        }
    }
}
