using Best.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Models.Img
{
    public class BestUserImg : Img
    {
        public virtual BestUser BestUser { get; set; }
    }
}
