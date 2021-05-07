using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Models.Img
{
    public class CampaignImg : Img
    {
        public virtual Campaign Campaign { get; set; }
    }
}
