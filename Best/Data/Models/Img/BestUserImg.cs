using Best.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Models.Img
{
    public class BestUserImg : Img
    {
        [ForeignKey("BestUser")]
        public string BestUserId { get; set; }
        public virtual BestUser BestUser { get; set; }
    }
}
