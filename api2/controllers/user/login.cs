using Microsoft.AspNetCore.Mvc;
using server.Mysql.Data;

namespace server.Controllers
{
    public partial class Api
    {
        // 使用属性注入
        [FromServices] public MysqlDbContext DbContext { get; set; }

        [HttpPost("user/LogIn")]
        public IActionResult SignUp([FromBody] LogInRequest request)
        {
            // 判断账号密码
            var user = DbContext.user2.FirstOrDefault(
                u => u.username == request.username && u.password == request.password);
            if (user == null)
                return Ok("账号或密码错误");
            return Ok("SignUp successful");
        }
    }

    public class LogInRequest
    {
        // [Required(ErrorMessage = "Account is required.")]
        // [RegularExpression("^[a-zA-Z0-9]{1,20}$", ErrorMessage = "账号只能是20位以内的数字或者字母.")]
        public string username { get; set; }
        public string password { get; set; }
    }
}