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
using Best.Data.Models.Combined;

namespace Best.Controllers
{
    public class PostsController : Controller
    {
        private readonly UserManager<BestUser> _userManager;
        private readonly SignInManager<BestUser> _signInManager;
        private readonly BestContent _context;
        private readonly IPosts _posts;
        private readonly ICampaings _campaings;

        public PostsController(BestContent context, IPosts posts, UserManager<BestUser> userManager, SignInManager<BestUser> signInManager, ICampaings campaings)
        {
            _context = context;
            _posts = posts;
            _userManager = userManager;
            _signInManager = signInManager;
            _campaings = campaings;
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
        public IActionResult Create(string id)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(Index));
            }
            if (id == null)
            {
                return View();
            }
            CombPost combPost = new CombPost();
            combPost.Campaing = _campaings.GetCampaingByIdForUser(_userManager.GetUserId(User), id);
            if (combPost == null)
            {
                return NotFound();
            }
            return View(combPost);
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CombPost combPost)
        {
            if (ModelState.IsValid)
            {
                Post post = combPost.Post;
                post.Campaing = _campaings.GetCampaingById(combPost.Campaing.Id);
                _context.Add(post);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(combPost);
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

            CombPost combPost = new CombPost();
            combPost.Post = _posts.GetPostByIdForUser(_userManager.GetUserId(User), id);
            if (combPost.Post == null)
            {
                return NotFound();
            }
            return View(combPost);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CombPost combPost)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(Index));
            }
            Post post = combPost.Post;
            post.Campaing = _campaings.GetCampaingById(combPost.Campaing.Id);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
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
            _context.Post.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(string id)
        {
            return _context.Post.Any(e => e.Id == id);
        }
    }
}
