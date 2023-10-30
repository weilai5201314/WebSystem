// Api.cs

using Microsoft.AspNetCore.Mvc;

namespace server.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public partial class Api : Controller
    {
        [FromServices] public IConfiguration Configuration { get; set; }
        
        // [FromServices] public MysqlDbContext DbContext { get; set; }
        // GET
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello,Api!");
        }
    }
}