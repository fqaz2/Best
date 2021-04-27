using Best.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Models.Combined
{
    public class CombPost
    {
        public Campaing Campaing { get; set; }
        public Post Post { get; set; }
        public BestUser BestUser { get; set; }
    }
}
