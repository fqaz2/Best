using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Best.Data;
using Best.Data.Models;
using Best.Data.Interfaces;
using Best.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Best.Data.Models.Like;

namespace Best.Controllers
{
    public class PostsController : Controller
    {
        private readonly UserManager<BestUser> _userManager;
        private readonly SignInManager<BestUser> _signInManager;
        private readonly BestContent _context;
        private readonly IPosts _posts;
        private readonly ICampaigns _Campaigns;
        private readonly IBestUsers _bestUsers;

        public PostsController(BestContent context, IPosts posts, UserManager<BestUser> userManager, SignInManager<BestUser> signInManager, ICampaigns Campaigns, IBestUsers bestUsers)
        {
            _context = context;
            _posts = posts;
            _userManager = userManager;
            _signInManager = signInManager;
            _Campaigns = Campaigns;
            _bestUsers = bestUsers;
        }

        // GET: Posts
        public async Task<IActionResult> Index(string id)
        {
            if (id == null)
            {
                return View(_posts.GetPosts.ToList());
            }
            return View(_posts.GetPostsByCampaignId(id).ToList());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = _posts.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public async Task<IActionResult> Create(string id)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(Index));
            }
            if (id == null)
            {
                return View();
            }
            Post post = new Post();
            post.Campaign = _Campaigns.GetCampaignByIdForUser(_userManager.GetUserId(User), id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post post)
        {
            if (ModelState.IsValid)
            {
                post.Campaign = _Campaigns.GetCampaignById(post.Campaign.Id);
                post.BestUser = await _userManager.FindByIdAsync(post.BestUser.Id);
                await _posts.Create(post);
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(Index));
            }
            if (id == null)
            {
                return NotFound();
            }

            Post post = new Post();
            post = _posts.GetPostByIdForUser(_userManager.GetUserId(User), id);

            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                await _posts.Update(post);
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id == null)
            {
                return NotFound();
            }

            Post post = _posts.GetPostByIdForUser(_userManager.GetUserId(User), id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var post = _posts.GetPostById(id);
            await _posts.Delete(post);
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(string id)
        {
            return _context.Post.Any(e => e.Id == id);
        }
        [HttpPost]
        public async Task<JsonResult> LikePost(string PostId, string UserId)
        {
            var result = _context.PostLike.Where(l => l.Post.Id == PostId && l.BestUser.Id == UserId).ToList();
            if (result.Count != 0)
            {
                _context.PostLike.RemoveRange(result);
                await _context.SaveChangesAsync();
                return Json(false);
            }
            PostLike like = new PostLike();
            like.Post = await _context.Post.FirstOrDefaultAsync(p => p.Id == PostId);
            like.BestUser = await _context.BestUser.FirstOrDefaultAsync(u => u.Id == UserId);
            _context.Add(like);
            await _context.SaveChangesAsync();
            return Json(true);
        }
    }
}
