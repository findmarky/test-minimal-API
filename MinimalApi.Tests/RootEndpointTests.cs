using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Xunit;

namespace MinimalApi.Tests
{
    public class RootEndpointTests
    {
        [Fact]
        public async Task Root_Endpoint_Should_Return_Hello_World()
        { 
            // The WebApplicationFactory<T> class creates an in-memory application. It handles
            // bootstrapping of the  application, and provides an HttpClient that you can use to make requests.
            await using var application = new WebApplicationFactory<Program>();

            using var client = application.CreateClient();

            var response = await client.GetStringAsync("/");

            Assert.Equal("Hello World!", response);
        }
    }
}