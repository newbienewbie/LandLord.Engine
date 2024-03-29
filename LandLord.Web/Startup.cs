using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using LandLord.Core.Repository;
using LandLord.Shared;
using LandLord.Shared.CardJsonConverters;
using LandLord.Shared.Hub.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LandLord.Web
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
            var playerJsonConverter= new PlayerCardJsonConverter();
            var playingJsonConverter= new PlayingCardJsonConverter();
            services.AddControllersWithViews().AddJsonOptions(opts => {
                opts.JsonSerializerOptions.Converters.Add(playingJsonConverter);
                opts.JsonSerializerOptions.Converters.Add(playerJsonConverter);
                opts.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });
            services.AddRazorPages();
            services.AddScoped(sp => new GameRoomRepository(Configuration["GameRoomDb:Name"], "rooms"));
            services.AddSignalR(o => {
                o.EnableDetailedErrors = true;
            })
                .AddJsonProtocol(opts => {
                    var converters = opts.PayloadSerializerOptions.Converters;
                    converters.Add(playingJsonConverter);
                    converters.Add(playerJsonConverter);
                    opts.PayloadSerializerOptions.PropertyNameCaseInsensitive = true;
                })
                ;
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
            app.UseClientSideBlazorFiles<LandLord.BlazorApp.Startup>();
            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<GameHub>("/gamehub");
                endpoints.MapFallbackToController("/room/{id:guid}","Index","Home");
            });
        }
    }
}
