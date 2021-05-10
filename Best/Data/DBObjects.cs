using Best.Data.Models;
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
            InitialTopics(content);
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
        private static void InitialTopics(BestContent content)
        {

            if (!content.Topic.Any())
            {
                Topic topic = new Topic();
                topic.Name = "It";
                topic.Description = "Information technology (IT) is the use of computers to store, retrieve, transmit, and manipulate data or information.";
                content.Topic.Add(topic);
                content.SaveChanges();
            }
        }
    }
}
