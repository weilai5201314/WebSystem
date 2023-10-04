using Microsoft.AspNetCore.Mvc;
using server.HashEncry;
using server.Mysql.Data;
using System.ComponentModel.DataAnnotations; // 导入数据注解命名空间

namespace server.Controllers.User
{
    public partial class Api
    {
        // private readonly MysqlDbContext _context; // 注入数据库上下文
        //
        // // public MysqlDbContext _context;
        // public Api(MysqlDbContext context)
        // {
        //     _context = context; // 注入数据库上下文
        // }
        
        // 使用属性注入
        [FromServices] 
        public MysqlDbContext SignUp_context { get; set; }

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
                return BadRequest(validationResults.Select(r => r.ErrorMessage));
            }

            // 判断是否已经注册
            if (SignUp_context.User.Any(u => u.Account == request.Account))
            {
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

            return Ok("注册成功，等待管理审核。");
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