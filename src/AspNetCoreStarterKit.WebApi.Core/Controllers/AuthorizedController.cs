using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreStarterKit.WebApi.Core.Controllers
{
    [Authorize]
    public class AuthorizedController : ApiControllerBase
    {
    }
}