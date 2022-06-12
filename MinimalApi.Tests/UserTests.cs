using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace MinimalApi.Tests
{
    public class UserTests
    {
        [Fact]
        public async Task Create_User_Should_Return_Created_When_User_Request_Is_Valid()
        {
            await using var application = new WebApplicationFactory<Program>();

            var client = application.CreateClient();

            var result = await client.PostAsJsonAsync("/user", new User
            {
                FirstName = "Jim",
                LastName = "Jones",
                Email = "jim@testemail.com"
            });

            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
        }

        [Fact]
        public async Task Create_User_Should_Return_BadRequest_When_User_Email_Not_Provided()
        {
            await using var application = new WebApplicationFactory<Program>();

            var client = application.CreateClient();

            var result = await client.PostAsJsonAsync("/user", new User
            {
                FirstName = "Jim",
                LastName = "Jones",
            });

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

            var problemDetails = await result.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();

            Assert.NotNull(problemDetails);      
            Assert.Equal("The Email field is required.", problemDetails!.Errors["Email"][0]);
        }
    }
}
