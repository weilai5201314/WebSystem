using Microsoft.AspNetCore.Mvc;

namespace server.Controllers.UserAll;

[ApiController]
[Route("[controller]")]
public class LogIn : Controller
{
    // GET
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello,LogIn");
    }

    
}