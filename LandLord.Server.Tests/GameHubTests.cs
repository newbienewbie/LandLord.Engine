using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Itminus.LandLord.Engine;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace LandLord.Server.Tests
{
    public class GameHubTests : IClassFixture<LandLordWebAppFactory>
    {
        private readonly LandLordWebAppFactory _factory;
        private TestServer _testServer;

        public GameHubTests(LandLordWebAppFactory factory)
        {
            this._factory = factory;
            _factory.CreateClient(); // need to create a client for the server property to be available
            this._testServer = _factory.Server;
        }

        [Fact]
        public async Task TestHubClient()
        {
            //var connection = await StartConnectionAsync(this._testServer.CreateHandler());
            //connection.Closed += async error =>
            //{
            //    await Task.Delay(new Random().Next(0, 5) * 1000);
            //    await connection.StartAsync();
            //};
            //connection.On<GameRoom>("ReceiveState", (room) => {
            //});
            //await connection.InvokeAsync("AddToRoom", roomId);
        }

        private async Task<HubConnection> StartConnectionAsync(HttpMessageHandler handler)
        {
            var hubConnection = new HubConnectionBuilder()
                .WithUrl($"ws://localhost/gamehub", o =>
                {
                    o.HttpMessageHandlerFactory = _ => handler;
                })
                .Build();

            await hubConnection.StartAsync();

            return hubConnection;
        }
    }
}
