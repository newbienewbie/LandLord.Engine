using System;
using LandLord.Web.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(LandLord.Web.Areas.Identity.IdentityHostingStartup))]
namespace LandLord.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<IdentityDataContext>(options => {
                    var conn = "Server=(localdb)\\mssqllocaldb;Database=LandLord.Web;Trusted_Connection=True;MultipleActiveResultSets=true";
                    //context.Configuration.GetConnectionString("IdentityDataContextConnection")));
                    options.UseSqlServer(conn);
                });

                services.AddDefaultIdentity<AppUser>(options => {
                    options.SignIn.RequireConfirmedAccount = false; 
                })
                .AddEntityFrameworkStores<IdentityDataContext>();
            });
        }
    }
}