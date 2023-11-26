using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using server.HashEncry;
using server.Mysql.Data;

namespace server.Controllers
{
    public partial class Api
    {
        [HttpPost("user/LogIn2")]
        public IActionResult LogIn([FromBody] LogInRequest2 request)
        {
            var user = DbContext.User.FirstOrDefault(u => u.Account == request.Account);

            if (user == null)
            {
                // 用户不存在，记录登录失败的日志
                TypeLog(null, "LogIn", true, request.Account, false, "Invalid username ");
                return BadRequest("Invalid username ");
            }

            // 判断账号状态，只有 2 正常
            // 判断状态能否登录
            if (user.Status == 2)
            {
                // 判断密码
                if (PasswordHelper.ComparePasswords(request.Password, user.Pass, user.R, 1))
                {
                    // 密码验证通过，创建 JWT Token
                    var token = GenerateJwtToken(user);
                    // 同时还要修改 n ,但是要先判断是不是1
                    // 如果是，直接用n2，r2覆盖，否则-1
                    if (user.N == 2)
                    {
                        user.N = user.N2;
                        user.R = user.R2;
                        user.Pass = Convert.FromBase64String(request.Password2);
                    }
                    else
                    {
                        user.N -= 1;
                        user.Pass = Convert.FromBase64String(request.Password2);
                    }


                    DbContext.User.Update(user);
                    DbContext.SaveChanges();
                    // 记录登录成功的日志
                    TypeLog(request.Account, "LogIn", true, request.Account, true, token);
                    // 返回成功登录的响应，包括 Token
                    return Ok(new { token });
                }

                // 密码错误
                TypeLog(request.Account, "LogIn", true, request.Account, false, "Invalid  password");
                return BadRequest("Invalid  password");
            }

            // 记录登录失败的日志
            TypeLog(request.Account, "LogIn", true, request.Account, false, "请等待管理审核。");
            return BadRequest("请等待管理审核。");
        }
    }

    // 用户登录请求模型
    public class LogInRequest2
    {
        [Required(ErrorMessage = "Account is required.")]
        [RegularExpression("^[a-zA-Z0-9]{1,20}$", ErrorMessage = "账号只能是20位以内的数字或者字母.")]
        public string Account { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression("^[a-zA-Z0-9!@#$%^&*()_+\\-=[\\]{};:'\\\",<.>/?]+$",
            ErrorMessage =
                "Password must be alphanumeric and may include special characters. It should be less than or equal to 30 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression("^[a-zA-Z0-9!@#$%^&*()_+\\-=[\\]{};:'\\\",<.>/?]+$",
            ErrorMessage =
                "Password must be alphanumeric and may include special characters. It should be less than or equal to 30 characters.")]
        public string Password2 { get; set; }
    }
}