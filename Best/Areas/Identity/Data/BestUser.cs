using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Best.Data.Models;
using Best.Data.Models.Img;
using Microsoft.AspNetCore.Identity;

namespace Best.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the BestUser class
    public class BestUser : IdentityUser
    {
        public bool IsBlock { get; set; }
        public string Img { get; set; }
        public virtual IEnumerable<Campaing> Campaings { get; set; }
        public virtual IEnumerable<Post> Posts { get; set; }
        public virtual IEnumerable<BestUserImg> Carousel { get; set; }
    }
}
