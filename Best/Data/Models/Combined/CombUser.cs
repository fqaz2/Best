using Best.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Models.Combined
{
    public class CombUser
    {
        public BestUser BestUser { get; set; }
        public IdentityRole IdentityRole { get; set; }
    }
}
