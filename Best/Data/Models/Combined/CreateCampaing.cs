using Best.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Models.Combined
{
    public class CreateCampaing
    {
        public Campaing Campaing { get; set; }
        public Topic Topic { get; set; }
        public BestUser BestUser { get; set; }
    }
}
