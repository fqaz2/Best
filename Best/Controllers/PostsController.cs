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
using Best.Data.Interfaces.IComment;

namespace Best.Controllers
{
    public class PostsController : Controller
    {
        private readonly UserManager<BestUser> _userManager;
        private readonly SignInManager<BestUser> _signInManager;
        private readonly BestContent _context;
        private readonly IPosts _posts;
        private readonly IPostsComments _postsComments;
        private readonly ICampaigns _Campaigns;
        private readonly IBestUsers _bestUsers;

        public PostsController(BestContent context, IPosts posts, UserManager<BestUser> userManager, SignInManager<BestUser> signInManager, ICampaigns Campaigns, IBestUsers bestUsers, IPostsComments postsComments)
        {
            _context = context;

            _posts = posts;
            _postsComments = postsComments;

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
            post.Comments = post.Comments.OrderBy(d => d.CreateData);
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
                return NotFound();
            }
            return View(new Post() { CampaignId = id});
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post post)
        {
            if (ModelState.IsValid)
            {
                await _posts.Create(post);
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || !_signInManager.IsSignedIn(User))
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
            if (!_signInManager.IsSignedIn(User) || id == null)
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
            await _posts.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(string id)
        {
            return _context.Post.Any(e => e.Id == id);
        }
        [HttpPost]
        public async Task<JsonResult> LikePost(string PostId, string UserId)
        {
            return Json(_posts.LikePost(PostId, UserId));
        }
        public async Task<JsonResult> AddComment(string Id, string UserId, string Text)
        {
            return Json(await _postsComments.AddComment(Id, UserId, Text));
        }
    }
}
