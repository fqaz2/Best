using System;
using Best.Areas.Identity.Data;
using Best.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Best.Areas.Identity.IdentityHostingStartup))]
namespace Best.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<BestContent>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("BestContextConnection")));

                services.AddDefaultIdentity<BestUser>(options => 
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.Password.RequiredLength = 1;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<BestContent>();
            });
        }
    }
}