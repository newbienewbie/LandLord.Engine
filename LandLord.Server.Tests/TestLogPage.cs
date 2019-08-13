using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LandLord.Server.Tests
{
    public class TestLogPage : IntegrationTestBase
    {
        [Fact]
        public async Task GetLoginPage()
        {
            using (var _client = this.TestServer.CreateClient())
            {
                var resp = await _client.GetAsync("/Identity/Account/Login");
                Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
            }
        }
    }
}
