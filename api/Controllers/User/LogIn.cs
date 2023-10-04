using Microsoft.AspNetCore.Mvc;

namespace server.Controllers.User
{
    public partial class Api
    {
        // private readonly Dictionary<string, (byte[] salt, byte[] hash)> users =
        //     new Dictionary<string, (byte[] salt, byte[] hash)>();

        [HttpPost("LogIn")]
        public IActionResult LogIn([FromBody] LogInRequest request)
        {
            // 现在这里的代码和之前一样
            return Ok("Hello,login");
        }
    }

    // 用户登录请求模型
    public class LogInRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}