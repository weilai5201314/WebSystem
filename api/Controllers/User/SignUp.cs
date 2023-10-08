using Microsoft.AspNetCore.Mvc;
using server.HashEncry;
using server.Mysql.Data;
using System.ComponentModel.DataAnnotations; // 导入数据注解命名空间
using server.Mysql.Models; // 导入Log实体类所在的命名空间

namespace server.Controllers.User
{
    public partial class Api
    {
        // 使用属性注入
        [FromServices] public MysqlDbContext SignUp_context { get; set; }

        
        [HttpPost("SignUp")]
        public IActionResult SignUp([FromBody] SignUpRequest request)
        {
            // 使用数据注解进行输入验证
            // 检测非法输入
            var validationContext = new ValidationContext(request, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(request, validationContext, validationResults,
                    validateAllProperties: true))
            {
                // 如果验证失败，返回错误消息
                LogSignUp(request.Account, false,$"{request.Account} {request.Password}", "非法输入。");
                return BadRequest(validationResults.Select(r => r.ErrorMessage));
            }

            // 判断是否已经注册
            if (SignUp_context.User.Any(u => u.Account == request.Account))
            {
                // 记录注册失败的日志
                LogSignUp(request.Account, false,$"{request.Account} {request.Password}", "Username already exists.");
                return BadRequest("Username already exists.");
            }

            // 使用 PasswordHelper 类的 GenerateSaltAndHash 方法生成盐值和哈希密码
            var (salt, hash) = PasswordHelper.GenerateSaltAndHash(request.Password);

            // 创建一个新的用户实体
            var newUser = new server.Mysql.Models.User
            {
                Account = request.Account,
                Pass = hash,
                Salt = salt,
                Status = 0,
                RevertPass = new byte[256]
            };

            // 将新用户添加到数据库
            SignUp_context.User.Add(newUser);
            SignUp_context.SaveChanges();

            // 记录注册成功的日志
            LogSignUp(request.Account, true,$"{request.Account} {request.Password}", "注册成功，等待管理审核。");

            return Ok("注册成功，等待管理审核。");
        }

// 添加记录注册操作的日志函数
        private void LogSignUp(string userName, bool success,string input, string message)
        {
            // 创建日志实体
            var log = new Log
            {
                Timestamp = DateTime.UtcNow,
                User = userName,
                Action = "SignUp",
                InputResult = success,
                InputValue = $"UserName: {userName}, Input:{input}",
                ReturnResult = success,
                ReturnValue = message // 这里可以记录注册成功的消息或 Token
            };

            // 将日志实体添加到数据库中
            SignUp_context.Log.Add(log);
            SignUp_context.SaveChanges();
        }
    }

    public class SignUpRequest
    {
        [Required(ErrorMessage = "Account is required.")]
        [RegularExpression("^[a-zA-Z0-9]{1,20}$", ErrorMessage = "账号只能是20位以内的数字或者字母.")]
        public string Account { get; set; }

        // 添加输入检测限制
        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression("^[a-zA-Z0-9!@#$%^&*()_+\\-=[\\]{};:'\\\",<.>/?]+$",
            ErrorMessage =
                "Password must be alphanumeric and may include special characters. It should be less than or equal to 30 characters.")]
        public string Password { get; set; }
    }
}