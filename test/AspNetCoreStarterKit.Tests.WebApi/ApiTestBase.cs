using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreStarterKit.Application.Authentication.Dto;
using AspNetCoreStarterKit.Tests.Shared;
using AspNetCoreStarterKit.Utilities.Extensions.PrimitiveTypes;
using Microsoft.AspNetCore.TestHost;

namespace AspNetCoreStarterKit.Tests.WebApi
{
    public class ApiTestBase : TestBase
    {
        protected readonly TestServer TestServer;

        public ApiTestBase()
        {
            TestServer = GetTestServer();
        }

        protected async Task<HttpResponseMessage> LoginAsync(string userNameOrEmail, string password)
        {
            var userLoginViewModel = new LoginInput
            {
                UserNameOrEmail = userNameOrEmail,
                Password = password
            };

            return await TestServer.CreateClient().PostAsync("/api/login",
                userLoginViewModel.ToStringContent(Encoding.UTF8, "application/json"));
        }
    }
}