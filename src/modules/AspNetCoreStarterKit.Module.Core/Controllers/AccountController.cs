using System.Collections.Generic;
using AspNetCoreStarterKit.WebApi.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreStarterKit.Module.Core.Controllers
{
    public class AccountController : ApiControllerBase
    {
        [HttpGet]
        public List<string> Get()
        {
            return new List<string>
            {
                "this", "is", "from", "core", "module"
            };
        }
    }
}
