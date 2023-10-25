using Microsoft.AspNetCore.Mvc;

namespace server.Controllers.User
{
    [ApiController]
    [Route("[controller]/user")]
    public partial class Api : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello,user!");
        }
    }
}