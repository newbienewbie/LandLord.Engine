using Itminus.LandLord.Engine;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LandLord.Server.Tests
{
    public class TestGameRoomController : IClassFixture<LandLordWebAppFactory>
    {
        private readonly LandLordWebAppFactory factory;
        private readonly TestServer _testServer;

        public TestGameRoomController(LandLordWebAppFactory factory)
        {
            this.factory = factory;
            this._testServer = factory.Server;
        }

        [Fact]
        public async Task TestGet()
        {
            try
            {
                using (var _client = this._testServer.CreateClient())
                {
                    var resp = await _client.GetAsync("/api/GameRoom");
                    Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
                    var str = await resp.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<GameRoom>>(str);
                }
            }
            catch (Exception ex) {
                throw new Xunit.Sdk.XunitException($"fail to get GameRoom : {ex.Message}");
            }
        }

        [Fact]
        public async Task TestPut()
        {
            try
            {
                using (var _client = this._testServer.CreateClient())
                {
                    var content = new StringContent(String.Empty);
                    var resp = await _client.PutAsync("/api/GameRoom", content );
                    Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
                    var str = await resp.Content.ReadAsStringAsync();
                    var room = JsonConvert.DeserializeObject<GameRoom>(str);
                    Assert.NotNull(room);
                    Assert.True(room.Id != default);  
                    Assert.Equal(0, room.Cards.Count);      // the cards should never be sent to the client
                }
            }
            catch (Exception ex) {
                throw new Xunit.Sdk.XunitException($"fail to create GameRoom : {ex.Message}");
            }

            try
            {
                using (var _client = this._testServer.CreateClient())
                {
                    var resp = await _client.GetAsync("/api/GameRoom");
                    Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
                    var str = await resp.Content.ReadAsStringAsync();
                    var rooms = JsonConvert.DeserializeObject<List<GameRoom>>(str);
                    Assert.NotNull(rooms);
                    Assert.True(rooms.Count > 0);
                    Assert.True(rooms[0].Id != default);
                }
            }
            catch (Exception ex) {
                throw new Xunit.Sdk.XunitException($"fail to retrieve GameRoom : {ex.Message}");
            }

        }
    }
}
