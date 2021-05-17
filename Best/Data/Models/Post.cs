using Best.Areas.Identity.Data;
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
        public string text { get; set; }
        public string mintext { get; set; }
        public DateTime createData { get; set; }
        [ForeignKey("Campaign")]
        public string CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }
        [ForeignKey("BestUser")]
        public string BestUserId { get; set; }
        public virtual BestUser BestUser { get; set; }
        public virtual IEnumerable<PostLike> Likes { get; set; }
        public virtual IEnumerable<PostImg> Carousel { get; set; }
        [NotMapped]
        public IFormFile ImgFile { get; set; }
        [NotMapped]
        public IEnumerable<IFormFile> ImgsFile { get; set; }
    }
}
