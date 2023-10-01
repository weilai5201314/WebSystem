using Microsoft.AspNetCore.Mvc;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class api : Controller
{
    // GET
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello,Api!");
    }
}