using Best.Areas.Identity.Data;
using Best.Data.Models.Img;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Models
{
    public class Campaign
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Img { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public string Bonuses { get; set; }
        public string TitleImg { get; set; }
        public IEnumerable<Post> Posts { get; set; }
        public virtual Topic Topic { get; set; }
        public virtual BestUser BestUser { get; set; }
        public virtual IEnumerable<CampaignImg> Carousel { get; set; }
        [NotMapped]
        public IFormFile ImgFile { get; set; }
        [NotMapped]
        public IEnumerable<IFormFile> ImgsFile { get; set; }
    }
}
