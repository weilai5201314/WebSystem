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

    // 简单计算
    [HttpGet("add")]
    public IActionResult Add(int num1, int num2)
    {
        int sum = num1 + num2;
        int sum2 = num1 * num2;
        var result = new
        {
            Sum = sum,
            Product = sum2
        };
        return Ok(result);
    }
}