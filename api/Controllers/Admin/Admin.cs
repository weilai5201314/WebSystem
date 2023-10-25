using Microsoft.AspNetCore.Mvc;

namespace server.Controllers.Admin
{
    [ApiController]
    [Route("[controller]/admin")]
    public partial class Api : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello,Admin!");
        }
    }
}