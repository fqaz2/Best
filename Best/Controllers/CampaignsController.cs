﻿using Best.Areas.Identity.Data;
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
            Campaign.Topic = _topics.GetTopicById(Campaign.Topic.Id);
            Campaign.BestUser = await _userManager.FindByIdAsync(Campaign.BestUser.Id);
            
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
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id == null)
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
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id == null)
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

            var Campaign = _Campaigns.GetCampaignById(id);

            if (Campaign.BestUser.Id != _userManager.GetUserId(User))
            {
                return RedirectToAction(nameof(Index));
            }

            await _Campaigns.Delete(Campaign);
            return RedirectToAction(nameof(Index));
        }
        private bool CampaignExists(string id)
        {
            return _context.Topic.Any(e => e.Id == id);
        }
    }
}