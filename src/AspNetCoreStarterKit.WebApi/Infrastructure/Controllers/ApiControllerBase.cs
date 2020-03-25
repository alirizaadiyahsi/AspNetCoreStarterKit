using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreStarterKit.WebApi.Infrastructure.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ApiControllerBase : Controller
    {
    }
}
