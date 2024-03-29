using Best.Areas.Identity.Data;
using Best.Data;
using Best.Data.Interfaces;
using Best.Data.Interfaces.IComment;
using Best.Data.Interfaces.IImg;
using Best.Data.Models.Comment;
using Best.Data.Repository;
using Best.Data.Repository.CommentRepository;
using Best.Data.Repository.ImgRepository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Best
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BestContent>(options => options.UseSqlServer(Configuration.GetConnectionString("BestContextConnection")));

            services.AddTransient<IBestUsers, BestUserRepository>();

            services.AddTransient<IDropbox, DropboxRepository>();

            services.AddTransient<ICampaigns, CampaignRepository>();
            services.AddTransient<ICampaignImg, CampaignImgRepository>();

            services.AddTransient<IPosts, PostRepository>();
            services.AddTransient<IPostImg, PostImgRepository>();
            services.AddTransient<IPostsComments, PostCommentRepository>();

            services.AddTransient<ITopics, TopicRepository>();

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddLocalization(Options=> { Options.ResourcesPath = "Resources"; });//add lang
            services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();
            services.AddControllersWithViews();

            services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    var supportCultures = new List<CultureInfo>
                    {
                        new CultureInfo("en"),
                        new CultureInfo("ru")
                    };
                    options.DefaultRequestCulture = new RequestCulture("en");
                    options.SupportedCultures = supportCultures;
                    options.SupportedUICultures = supportCultures;
                });

            services.AddAuthentication()
                .AddGoogle(Options =>
            {
                Options.ClientId = Configuration["Authentication:Google:ClientId"];
                Options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            })
                .AddFacebook(Options =>
                {
                    Options.ClientId = Configuration["Authentication:Facebook:ClientId"];
                    Options.ClientSecret = Configuration["Authentication:Facebook:ClientSecret"];
                });

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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            //var supportedCultures = new[] { "en", "ru" };//add lang
            //var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
            //    .AddSupportedCultures(supportedCultures)
            //    .AddSupportedUICultures(supportedCultures);

            //app.UseRequestLocalization(localizationOptions);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            //add role administrator
            using (var scope = app.ApplicationServices.CreateScope())
            {
                BestContent content = scope.ServiceProvider.GetRequiredService<BestContent>();
                DBObjects.Initial(content);
            }
        }
    }
}
//1. need refactoring code