using Best.Areas.Identity.Data;
using Best.Data;
using Best.Data.Interfaces;
using Best.Data.Models;
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
    public class CampaingsController : Controller
    {
        private readonly ICampaings _campaings;
        private readonly ITopics _topics;
        private readonly UserManager<BestUser> _userManager;
        private readonly SignInManager<BestUser> _signInManager;
        private readonly BestContent _context;
        public CampaingsController(ICampaings campaings, ITopics topics, UserManager<BestUser> userManager, SignInManager<BestUser> signInManager, BestContent context)
        {
            _campaings = campaings;
            _topics = topics;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        public IEnumerable<Campaing> GetCampaings()
        {
            return _campaings.GetCampaings;
        }
        public IActionResult Index()
        {
            return View(_campaings.GetCampaings);
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
        public async Task<IActionResult> Create(Campaing campaing)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(Index));
            }
            campaing.Topic = _topics.GetTopicById(campaing.Topic.Id);
            campaing.BestUser = await _userManager.FindByIdAsync(campaing.BestUser.Id);
            
            await _campaings.Create(campaing);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Campaing = _campaings.GetCampaingById(id);
            if (Campaing == null)
            {
                return NotFound();
            }

            return View(Campaing);
        }
        public IActionResult Edit(string id)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id == null)
            {
                return NotFound();
            }

            var campaing = _campaings.GetCampaingByIdForUser(_userManager.GetUserId(User), id);
            if (campaing == null)
            {
                return NotFound();
            }
            return View(campaing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Campaing campaing)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(Index));
            }

            campaing.Topic = _topics.GetTopicById(campaing.Topic.Id);
            campaing.BestUser = await _userManager.FindByIdAsync(campaing.BestUser.Id);
            await _campaings.Update(campaing);

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(campaing);
        }
        public IActionResult Delete(string id)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id == null)
            {
                return NotFound();
            }

            var campaing = _campaings.GetCampaingByIdForUser(_userManager.GetUserId(User), id);
            if (campaing == null)
            {
                return NotFound();
            }

            return View(campaing);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(Index));
            }

            var campaing = _campaings.GetCampaingById(id);

            if (campaing.BestUser.Id != _userManager.GetUserId(User))
            {
                return RedirectToAction(nameof(Index));
            }

            await _campaings.Delete(campaing);
            return RedirectToAction(nameof(Index));
        }
        private bool CampaingExists(string id)
        {
            return _context.Topic.Any(e => e.Id == id);
        }
    }
}
