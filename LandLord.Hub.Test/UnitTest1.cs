//using LandLord.Shared.Hub.Services;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.TestHost;
//using Microsoft.AspNetCore.SignalR;
//using System;
//using Xunit;
//using Microsoft.AspNetCore.SignalR.Client;

//namespace LandLord.Hub.Test
//{
//    public class UnitTest1
//    {
//        [Fact]
//        public void Test1()
//        {
//            var webHostBuilder = new WebHostBuilder()
//                .ConfigureServices(services =>
//                {
//                    services.AddSignalR();
//                })
//                .Configure(app =>
//                {
//                    app.UseSignalR(routes => routes.MapHub<GameHub>("/gamehub"));
//                });

//            var server = new TestServer(webHostBuilder);
//            var connection = new HubConnectionBuilder()
//                .WithUrl(
//                    "http://localhost/echo",
//                    o => o.HttpMessageHandlerFactory = _ => server.CreateHandler()
//                )
//                .Build();

//        }
//    }
//}
