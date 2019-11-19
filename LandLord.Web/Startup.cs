using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using JsonSubTypes;
using LandLord.Core.Repository;
using LandLord.Shared;
using LandLord.Shared.Hub.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

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
            var playerCardJsonConverter = JsonSubtypesConverterBuilder
                .Of(typeof(PlayerCard), "Kind") // type property is only defined here
                .RegisterSubtype(typeof(NormalCard), PlayerCardKind.NormalCard)
                .RegisterSubtype(typeof(BlackJokerCard), PlayerCardKind.BlackJokerCard)
                .RegisterSubtype(typeof(RedJokerCard), PlayerCardKind.RedJokerCard)
                .RegisterSubtype(typeof(Shadowed), PlayerCardKind.Shadowed)
                .SerializeDiscriminatorProperty() // ask to serialize the type property
                .Build();
            var playingCardJsonConverter = JsonSubtypesConverterBuilder
                .Of(typeof(PlayingCard), "Kind") // type property is only defined here
                .RegisterSubtype(typeof(NormalCard), PlayerCardKind.NormalCard)
                .RegisterSubtype(typeof(BlackJokerCard), PlayerCardKind.BlackJokerCard)
                .RegisterSubtype(typeof(RedJokerCard), PlayerCardKind.RedJokerCard)
                .SerializeDiscriminatorProperty() // ask to serialize the type property
                .Build();
            services.AddControllersWithViews().AddNewtonsoftJson(opts => {
                var converters = opts.SerializerSettings.Converters;
                converters.Clear();
                converters.Add(playerCardJsonConverter); 
                converters.Add(playingCardJsonConverter); 
            });
            services.AddRazorPages();
            services.AddScoped(sp => new GameRoomRepository(Configuration["GameRoomDb:Name"], "rooms"));
            services.AddSignalR(o => {
                o.EnableDetailedErrors = true;
            })
                .AddNewtonsoftJsonProtocol(opts => {
                    var converters = opts.PayloadSerializerSettings.Converters;
                    converters.Clear();
                    converters.Add(playerCardJsonConverter); 
                    converters.Add(playingCardJsonConverter); 
                })
                //.AddJsonProtocol(opts =>
                //{
                //    opts.PayloadSerializerOptions.PropertyNameCaseInsensitive = false;
                //    opts.PayloadSerializerOptions.PropertyNamingPolicy = null;
                //})
                ;
            //.AddNewtonsoftJsonProtocol();
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
