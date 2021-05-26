using Best.Data.Models.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Interfaces.IComment
{
    public interface IPostsComments
    {
        IEnumerable<PostComment> GetComments { get; }
        public PostComment GetCommentById(string comment_id);
        public Task<bool> AddComment(string Id, string UserId, string Text);
    }
}
