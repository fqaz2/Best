using Best.Areas.Identity.Data;
using Best.Data.Models.Img;
using Best.Data.Models.Rating;
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
        public string Bonuses { get; set; }
        public string shortText { get; set; }
        public string text { get; set; }
        public DateTime createData { get; set; }
        public IEnumerable<Post> Posts { get; set; }
        [ForeignKey("Topic")]
        public string TopicId { get; set; }
        public virtual Topic Topic { get; set; }
        [ForeignKey("BestUser")]
        public string BestUserId { get; set; }
        public virtual BestUser BestUser { get; set; }
        public virtual IEnumerable<CampaignImg> Carousel { get; set; }
        public virtual IEnumerable<CampaignRating> Ratings { get; set; }
        [NotMapped]
        public IFormFile ImgFile { get; set; }
        [NotMapped]
        public IEnumerable<IFormFile> ImgsFile { get; set; }
    }
}
