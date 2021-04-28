using Best.Areas.Identity.Data;
using Best.Data;
using Best.Data.Interfaces;
using Best.Data.Models;
using Best.Data.Models.Combined;
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
        public async Task<IActionResult> Create(CombCampaing combCampaing)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(Index));
            }
            Campaing campaing = combCampaing.Campaing;
            try
            {
                campaing.Topic = _topics.GetTopicById(combCampaing.Topic.Id);
                campaing.BestUserId = combCampaing.BestUser.Id;

                await _campaings.Create(campaing);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(combCampaing);
            }
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Campaing = await _context.Campaing
                .FirstOrDefaultAsync(m => m.Id == id);
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

            var Campaing = _campaings.GetCampaingByIdForUser(_userManager.GetUserId(User), id);
            if (Campaing == null)
            {
                return NotFound();
            }
            CombCampaing сombCampaing = new CombCampaing();//плохой код
            сombCampaing.Campaing = Campaing;
            return View(сombCampaing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CombCampaing combCampaing)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(Index));
            }

            combCampaing.Topic = _topics.GetTopicById(combCampaing.Topic.Id);
            Campaing campaing = combCampaing.Campaing;
            campaing.Topic = combCampaing.Topic;
            IEnumerable<Campaing> camp = from c in _context.Campaing //select all campaing without input campaing
                                  where c.Id != campaing.Id 
                                  select c; 

            if (ModelState.IsValid && !camp.Any(c => c.Name == campaing.Name))
            {
                try
                {
                    await _campaings.Update(campaing);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CampaingExists(campaing.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(combCampaing);
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

            var campaing = await _context.Campaing.FindAsync(id);

            if (campaing.BestUserId != _userManager.GetUserId(User))
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
