using Best.Data.Interfaces.IComment;
using Best.Data.Models.Comment;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Repository.CommentRepository
{
    public class PostCommentRepository : IPostsComments
    {
        private readonly BestContent _context;
        public PostCommentRepository(BestContent context)
        {
            _context = context;
        }
        public IEnumerable<PostComment> GetComments => _context.PostComments.Include(u => u.BestUser).Include(p => p.Post);

        public PostComment GetCommentById(string comment_id) => GetComments.SingleOrDefault(c => c.Id == comment_id);
        public async Task<bool> AddComment(string id, string userId, string text)
        {
            await _context.PostComments.AddAsync(new PostComment() { 
            PostId = id,
            BestUserId = userId,
            Text = text,
            CreateData = DateTime.Now
            });
            int res = await _context.SaveChangesAsync();
            return res > 0 ? true : false;
        }
    }
}
