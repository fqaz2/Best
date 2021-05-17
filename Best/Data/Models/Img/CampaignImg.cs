using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Models.Img
{
    public class CampaignImg : Img
    {
        [ForeignKey("Campaign")]
        public string CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }
    }
}
