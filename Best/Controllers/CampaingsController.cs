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
    public class CampaingsController : Controller
    {
        private readonly ICampaings _campaings;
        private readonly ITopics _topics;
        private readonly UserManager<BestUser> _userManager;
        public CampaingsController(ICampaings campaings, ITopics topics, UserManager<BestUser> userManager)
        {
            _campaings = campaings;
            _topics = topics;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View(_campaings.GetCampaings);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCampaing createCampaing)
        {
            try
            {
                Campaing campaing = createCampaing.Campaing;
                campaing.Topic = _topics.GetTopicById(createCampaing.Topic.Id);
                campaing.BestUser = await _userManager.FindByIdAsync(createCampaing.BestUser.Id);
                await _campaings.Add(campaing);
                return View();
            }
            catch
            {
                return View("index");
            }
        }
    }
}
