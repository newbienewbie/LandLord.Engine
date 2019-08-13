using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace LandLord.Server.Tests
{
    public class TestLogPage : IClassFixture<LandLordWebAppFactory>
    {
        private readonly LandLordWebAppFactory factory;
        private readonly TestServer _testServer;

        public TestLogPage(LandLordWebAppFactory factory)
        {
            this.factory = factory;
            this._testServer = factory.Server;
        }

        [Fact]
        public async Task GetLoginPage()
        {
            using (var _client = this._testServer.CreateClient())
            {
                var resp = await _client.GetAsync("/Identity/Account/Login");
                Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
            }
        }
    }
}
