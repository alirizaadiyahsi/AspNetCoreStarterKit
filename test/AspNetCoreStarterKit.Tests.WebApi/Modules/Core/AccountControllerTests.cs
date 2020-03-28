using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreStarterKit.Application.Authentication.Dto;
using AspNetCoreStarterKit.EntityFramework.DataSeeder;
using AspNetCoreStarterKit.Utilities.Extensions.PrimitiveTypes;
using Xunit;

namespace AspNetCoreStarterKit.Tests.WebApi.Modules.Core
{
    public class AccountControllerTests : ApiTestBase
    {
        [Fact]
        public async Task Should_Login()
        {
            var responseLogin = await LoginAsync(DbContextDataSeeder.AdminUserName, "123qwe");
            var loginResult = await responseLogin.Content.ReadAsAsync<LoginOutput>();
            Assert.Equal(HttpStatusCode.OK, responseLogin.StatusCode);
            Assert.NotNull(loginResult.Token);
        }

        [Fact]
        public async Task Should_Not_Login_With_Wrong_Credentials()
        {
            var responseLogin = await LoginAsync("wrongUserName", "wrongPassword");
            Assert.Equal(HttpStatusCode.NotFound, responseLogin.StatusCode);
        }

        [Fact]
        public async Task Should_Register()
        {
            var registerInput = new RegisterInput
            {
                Email = "TestUserEmail_" + Guid.NewGuid() + "@mail.com",
                UserName = "TestUserName_" + Guid.NewGuid(),
                Password = "aA!121212"
            };

            var responseRegister = await TestServer.CreateClient().PostAsync("/api/register",
                registerInput.ToStringContent(Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.OK, responseRegister.StatusCode);
        }

        [Fact]
        public async Task Should_Not_Register_With_Existing_User()
        {
            var registerInput = new RegisterInput
            {
                Email = DbContextDataSeeder.AdminUserEmail,
                UserName = DbContextDataSeeder.AdminUserName,
                Password = "aA!121212"
            };

            var responseRegister = await TestServer.CreateClient().PostAsync("/api/register",
                registerInput.ToStringContent(Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.Conflict, responseRegister.StatusCode);
        }

        [Fact]
        public async Task Should_Not_Register_With_Invalid_User()
        {
            var input = new RegisterInput
            {
                Email = new string('*', 300),
                UserName = new string('*', 300),
                Password = "aA!121212"
            };

            var response = await TestServer.CreateClient().PostAsync("/api/register",
                input.ToStringContent(Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
