using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data
{
    public class DBObjects
    {
        public static void Initial(BestContent content)
        {
            InitialRole(content);
        }
        private static void InitialRole(BestContent content)
        {
            
            if (!content.Roles.Any(r => r.Name == "Administrator"))
            {
                IdentityRole identityRole = new IdentityRole();
                identityRole.Name = "Administrator";
                identityRole.NormalizedName = identityRole.Name.ToUpper();
                content.Roles.Add(identityRole);
                content.SaveChanges();
            }
        }
    }
}
