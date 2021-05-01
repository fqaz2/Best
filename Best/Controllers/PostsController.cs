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

namespace Best.Controllers
{
    public class PostsController : Controller
    {
        private readonly UserManager<BestUser> _userManager;
        private readonly SignInManager<BestUser> _signInManager;
        private readonly BestContent _context;
        private readonly IPosts _posts;
        private readonly ICampaings _campaings;
        private readonly IBestUsers _bestUsers;

        public PostsController(BestContent context, IPosts posts, UserManager<BestUser> userManager, SignInManager<BestUser> signInManager, ICampaings campaings, IBestUsers bestUsers)
        {
            _context = context;
            _posts = posts;
            _userManager = userManager;
            _signInManager = signInManager;
            _campaings = campaings;
            _bestUsers = bestUsers;
        }

        // GET: Posts
        public async Task<IActionResult> Index(string id)
        {
            if (id == null)
            {
                return View(await _context.Post.ToListAsync());
            }
            return View(await _context.Post.Where(p => p.Campaing.Id == id).ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.Id == id);
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
            post.Campaing = _campaings.GetCampaingByIdForUser(_userManager.GetUserId(User), id);
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
                post.Campaing = _campaings.GetCampaingById(post.Campaing.Id);
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
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(Index));
            }

            post.Campaing = await _context.Campaing.FirstOrDefaultAsync(c => c.Id == post.Campaing.Id);
            post.BestUser = await _context.BestUser.FirstOrDefaultAsync(u => u.Id == post.BestUser.Id);

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
            var post = await _context.Post.FindAsync(id);
            await _posts.Delete(post);
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(string id)
        {
            return _context.Post.Any(e => e.Id == id);
        }
    }
}
