using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Models
{
    public class CampaingCarousel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Img { get; set; }
        public string Alt { get; set; }
        public virtual Campaing Campaing { get; set; }
    }
}
