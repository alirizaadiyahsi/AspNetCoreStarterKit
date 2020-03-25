using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreStarterKit.WebApi.Infrastructure.Controllers
{
    [Authorize]
    public class AuthorizedController : ApiControllerBase
    {
    }
}