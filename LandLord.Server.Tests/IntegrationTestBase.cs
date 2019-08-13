using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using Xunit;

namespace LandLord.Server.Tests
{
    public class IntegrationTestBase : IDisposable
    {
        public IConfigurationRoot Config { get; }
        public TestServer TestServer { get; }

        public IntegrationTestBase()
        {
            var p = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var testingConfig = new Dictionary<string, string> {
                {"GameRoomDb:Name", Path.Combine(p,"TestingGameRooms.db")},
            };
            this.Config = new ConfigurationBuilder()
                .SetBasePath(p)
                .AddJsonFile("appsettings.Test.json")
                .AddInMemoryCollection(testingConfig)
                .Build();

            var hb = new WebHostBuilder()
                .UseConfiguration(this.Config)
                .UseStartup<LandLord.Server.Startup>()
                ;
            this.TestServer = new TestServer(hb);
        }

        public void Dispose()
        {
            this.TestServer.Dispose();
        }

    }
}
