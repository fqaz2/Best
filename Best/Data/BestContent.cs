using Best.Areas.Identity.Data;
using Best.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data
{
    public class BestContent : IdentityDbContext<BestUser>
    {
        public BestContent(DbContextOptions<BestContent> options) : base(options)
        {

        }
        public DbSet<Campaing> Campaing { get; set; }
        public DbSet<Topic> Topic { get; set; }
    }
}
