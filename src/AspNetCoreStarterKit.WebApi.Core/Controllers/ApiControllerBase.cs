using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreStarterKit.WebApi.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ApiControllerBase : ControllerBase
    {
    }
}
