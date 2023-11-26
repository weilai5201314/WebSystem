using Microsoft.AspNetCore.Mvc;
using server.HashEncry;
using server.Mysql.Data;
using System.ComponentModel.DataAnnotations;

namespace server.Controllers
{
    public partial class Api
    {
        // 使用属性注入
        [FromServices] public MysqlDbContext SignUp_context { get; set; }

        [HttpPost("user/SignUp")]
        public IActionResult SignUp([FromBody] SignUpRequest request)
        {
            // 使用数据注解进行输入验证
            //   检测非法输入
            var validationContext = new ValidationContext(request, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(request, validationContext, validationResults,
                    validateAllProperties: true))
            {
                // 如果验证失败，返回错误消息
                TypeLog(request.Account, "Signup", true, $"{request.Account} ", false, "非法输入。");
                return BadRequest(validationResults.Select(r => r.ErrorMessage));
            }

            // 判断是否已经注册
            if (SignUp_context.User.Any(u => u.Account == request.Account))
            {
                TypeLog(request.Account, "Signup", true, $"{request.Account} ", false, "Username already exists.");
                return BadRequest("Username already exists.");
            }

            //  通过验证，开始生成返回值
            //  生成迭代次数和随机数
            int n = PasswordHelper.GetRandomN();
            byte[] r = PasswordHelper.GenerateRandomR();
            var newUser = new server.Mysql.Models.User
            {
                Account = request.Account,
                Pass = new byte[256],
                Salt = new byte[256],
                Status = 0,
                RevertPass = new byte[256],
                N = n,
                R = r,
                N2 = 0,
                R2 = new byte[32]
            };
            // 将新用户添加到数据库
            SignUp_context.User.Add(newUser);
            SignUp_context.SaveChanges();

            //  构造返回响应
            var response = new
            {
                N = n,
                R = r
            };
            // 记录注册成功的日志
            TypeLog(request.Account, "SignUp", true, $"{request.Account} ", true, $"{response}");

            return Ok(response);
        }
    }

    public class SignUpRequest
    {
        [Required(ErrorMessage = "Account is required.")]
        [RegularExpression("^[a-zA-Z0-9]{1,20}$", ErrorMessage = "账号只能是20位以内的数字或者字母.")]
        public string Account { get; set; }
    }
}