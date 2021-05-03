﻿using Best.Areas.Identity.Data;
using Best.Data.Models;
using Best.Data.Models.Img;
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
        //user
        public DbSet<BestUser> BestUser { get; set; }
        public DbSet<BestUserImg> BestUserImg { get; set; }
        //campaign
        public DbSet<Campaing> Campaing { get; set; }
        public DbSet<CampaingImg> CampaingImg { get; set; }
        public DbSet<Topic> Topic { get; set; }
        //post
        public DbSet<Post> Post { get; set; }
        public DbSet<PostImg> PostImg { get; set; }
        //post
        //campaign
        //user
    }
}
