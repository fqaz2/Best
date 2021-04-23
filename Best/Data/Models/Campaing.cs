using Best.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Models
{
    public class Campaing
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public string Bonuses { get; set; }
        public virtual BestUser BestUser { get; set; }
        public virtual Topic Topic { get; set; }
    }
}
