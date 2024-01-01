using System.ComponentModel.DataAnnotations;
// using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using server.HashEncry;

namespace server.Controllers
{
    public partial class Api
    {
        [HttpPost("user/LogIn")]
        public IActionResult LogIn([FromBody] LogInRequest request)
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
                // 开始读取数据库中的 n 和 r 并返回
                //  需要判断是不是最后一次,
                if (user.N == 2) // 如果是最后一次
                {
                    //  生成新的 n 和 r
                    int newn2 = PasswordHelper.GetRandomN();
                    byte[] newr2 = PasswordHelper.GenerateRandomR();
                    // 保存到数据库
                    user.N2 = newn2;
                    user.R2 = newr2;
                    DbContext.User.Update(user);
                    DbContext.SaveChanges();
                    //  构建返回
                    var response = new
                    {
                        N = user.N,
                        R = user.R,
                        n2 = newn2,
                        r2 = newr2
                    };
                    // 记录登录成功的日志
                    TypeLog(request.Account, "LogIn", true, $"{request.Account} ", true, $"{response}");
                    // 返回成功登录的响应
                    return Ok(new { response });
                }
                else
                {
                    var response = new
                    {
                        N = user.N,
                        R = user.R,
                        n2 = 0,
                        r2 = "0"
                    };
                    // 记录登录成功的日志
                    TypeLog(request.Account, "LogIn", true, $"{request.Account} ", true, $"{response}");
                    // 返回成功登录的响应
                    return Ok(new { response });
                }
                
            }

            // 记录登录失败的日志
            TypeLog(request.Account, "LogIn", true, request.Account, false, "请等待管理审核。");
            return BadRequest("请等待管理审核。");
        }
    }

    // 用户登录请求模型
    public class LogInRequest
    {
        [Required(ErrorMessage = "Account is required.")]
        [RegularExpression("^[a-zA-Z0-9]{1,20}$", ErrorMessage = "账号只能是20位以内的数字或者字母.")]
        public string Account { get; set; }
    }
}