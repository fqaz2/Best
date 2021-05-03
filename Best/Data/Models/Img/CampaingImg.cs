using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Models.Img
{
    public class CampaingImg : Img
    {
        public virtual Campaing Campaing { get; set; }
    }
}
