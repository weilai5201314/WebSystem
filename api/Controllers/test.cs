using Microsoft.AspNetCore.Mvc;

namespace server.Controllers
{
    public partial class Api 
    {
        
        [HttpGet("test/add")]
        public IActionResult SignUp(int num1, int num2)
        {
            int sum = num1 + num2;
            var result = new { Sum = sum };
            return Ok(result);
        }

        [HttpGet("test/mul")]
        public IActionResult LogIn(int num1, int num2)
        {
            int sum2 = num1 * num2;
            var result2 = new { Product = sum2 };
            return Ok(result2);
        }
    }
}