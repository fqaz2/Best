﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Models
{
    public class Post
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string text { get; set; }
        public string mintext { get; set; }
        public virtual Campaing Campaing { get; set; }
    }
}