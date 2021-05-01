using Best.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Models.Сombined
{
    public class CombUserRole
    {
        public virtual BestUser BestUser { get; set; }
        public virtual IdentityRole IdentityRole { get; set; }
    }
}
