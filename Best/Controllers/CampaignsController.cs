using Best.Areas.Identity.Data;
using Best.Data;
using Best.Data.Interfaces;
using Best.Data.Models;
using Best.Data.Models.Rating;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Controllers
{
    public class CampaignsController : Controller
    {
        private readonly ICampaigns _Campaigns;
        private readonly ITopics _topics;
        private readonly UserManager<BestUser> _userManager;
        private readonly SignInManager<BestUser> _signInManager;
        private readonly BestContent _context;
        public CampaignsController(ICampaigns Campaigns, ITopics topics, UserManager<BestUser> userManager, SignInManager<BestUser> signInManager, BestContent context)
        {
            _Campaigns = Campaigns;
            _topics = topics;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        public IEnumerable<Campaign> GetCampaigns()
        {
            return _Campaigns.GetCampaigns;
        }
        public IActionResult Index()
        {
            return View(_Campaigns.GetCampaigns);
        }
        public IActionResult Create()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Campaign Campaign)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(Index));
            }

            await _Campaigns.Create(Campaign);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Campaign = _Campaigns.GetCampaignById(id);

            if (Campaign == null)
            {
                return NotFound();
            }

            return View(Campaign);
        }
        public IActionResult Edit(string id)
        {
            if (!_signInManager.IsSignedIn(User) || id == null)
            {
                return NotFound();
            }

            var Campaign = _Campaigns.GetCampaignByIdForUser(_userManager.GetUserId(User), id);

            if (Campaign == null)
            {
                return NotFound();
            }

            return View(Campaign);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Campaign Campaign)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(Index));
            }

            await _Campaigns.Update(Campaign);

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(Campaign);
        }
        public IActionResult Delete(string id)
        {
            if (!_signInManager.IsSignedIn(User) || id == null)
            {
                return NotFound();
            }

            var Campaign = _Campaigns.GetCampaignByIdForUser(_userManager.GetUserId(User), id);

            if (Campaign == null)
            {
                return NotFound();
            }

            return View(Campaign);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(Index));
            }

            await _Campaigns.Delete(id);

            return RedirectToAction(nameof(Index));
        }
        private bool CampaignExists(string id)
        {
            return _context.Topic.Any(e => e.Id == id);
        }
        public async Task<JsonResult> CampaignRating(string CampaignId, string UserId, string Rating)
        {
            var result = _context.CampaignRating.Where(l => l.Campaign.Id == CampaignId && l.BestUser.Id == UserId).ToList();
            if (result.Count != 0)
            {
                _context.CampaignRating.RemoveRange(result);
                await _context.SaveChangesAsync();
                return Json(false);
            }
            CampaignRating campaignRating = new CampaignRating();
            campaignRating.Campaign = await _context.Campaign.FirstOrDefaultAsync(p => p.Id == CampaignId);
            campaignRating.BestUser = await _context.BestUser.FirstOrDefaultAsync(u => u.Id == UserId);
            campaignRating.Rating = Convert.ToInt32(Rating);
            _context.Add(campaignRating);
            await _context.SaveChangesAsync();
            return Json(true);
        }
    }
}
