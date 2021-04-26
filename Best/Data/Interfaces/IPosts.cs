using Best.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Interfaces
{
    public interface IPosts
    {
        IEnumerable<Post> GetPosts { get; }
        IEnumerable<Post> GetPostsByCampaingId(string campaing_Id);
        Post GetPostById(string post_id);
    }
}
