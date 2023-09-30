using Microsoft.AspNetCore.Mvc;

namespace server.Controllers.UserAll;
[ApiController]
[Route("[controller]")]
public class SignUp : Controller
{
    // GET
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello,SignUp");
    }
}