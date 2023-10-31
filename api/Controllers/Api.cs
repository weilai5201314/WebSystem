// Api.cs

using Microsoft.AspNetCore.Mvc;

namespace server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class Api : Controller
    {
        // GET
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello,Api!");
        }
    }
}