﻿using Best.Areas.Identity.Data;
using Best.Data.Models.Comment;
using Best.Data.Models.Img;
using Best.Data.Models.Like;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Models
{
    public class Post
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Img { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string ShortText { get; set; }
        public DateTime CreateData { get; set; }
        [ForeignKey("Campaign")]
        public string CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }
        [ForeignKey("BestUser")]
        public string BestUserId { get; set; }
        public virtual BestUser BestUser { get; set; }
        public virtual IEnumerable<PostLike> Likes { get; set; }
        public virtual IEnumerable<PostImg> Carousel { get; set; }
        public virtual IEnumerable<PostComment> Comments { get; set; }
        [NotMapped]
        public IFormFile ImgFile { get; set; }
        [NotMapped]
        public IEnumerable<IFormFile> ImgsFile { get; set; }
    }
}
