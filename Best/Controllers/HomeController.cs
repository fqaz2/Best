using Best.Data;
using Best.Data.Interfaces;
using Best.Data.Models;
using Best.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BestContent _context;
        private readonly ICampaigns _campaigns;
        private readonly IPosts _posts;

        public HomeController(ILogger<HomeController> logger, BestContent context, ICampaigns campaigns, IPosts posts)
        {
            _logger = logger;
            _context = context;
            _campaigns = campaigns;
            _posts = posts;
        }

        public IActionResult Index()
        {
            List<ObjectTime> list = new List<ObjectTime>();

            _posts.GetPosts.ToList().ForEach(p => list.Add(new ObjectTime() { Obj = p, CreateDate = p.createData, TypeObj = p.GetType() }));
            _campaigns.GetCampaigns.ToList().ForEach(c => list.Add(new ObjectTime() { Obj = c, CreateDate = c.createData, TypeObj = c.GetType() }));

            return View(list.OrderBy(l => l.CreateDate));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
