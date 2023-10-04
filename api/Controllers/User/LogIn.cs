using Microsoft.AspNetCore.Mvc;
using server.HashEncry;
using server.Mysql.Data;
// using server.Mysql.Models;
// using System.Linq; // 导入 LINQ 查询命名空间
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
// using Microsoft.Extensions.Configuration;
// using System;
// using Microsoft.AspNetCore.Mvc;
// using server.HashEncry;
// using server.Mysql.Data;
using System.ComponentModel.DataAnnotations; // 导入数据注解命名空间

namespace server.Controllers.User
{
    public partial class Api
    {
        // private readonly MysqlDbContext _context2; // 注入数据库上下文
        // private readonly IConfiguration _configuration2; // 注入配置

        // public Api(MysqlDbContext context, IServiceProvider serviceProvider)
        // {
        //     _context2 = context;
        //     _configuration2 = serviceProvider.GetRequiredService<IConfiguration>();
        // }

        // 构造注入
        [FromServices] public MysqlDbContext DbContext { get; set; }

        [FromServices] public IConfiguration Configuration { get; set; }

        [HttpPost("LogIn")]
        public IActionResult LogIn([FromBody] LogInRequest request)
        {
            // 根据用户名从数据库中查找用户
            var user = DbContext.User.FirstOrDefault(u => u.Account == request.Account);

            // 检查用户是否存在
            if (user == null)
            {
                return BadRequest("Invalid username or password");
            }

            // 判断账号状态，只有 2 正常
            if (user.Status == 2)
            {
                // 验证密码
                if (PasswordHelper.VerifyPassword(request.Password, user.Salt, user.Pass))
                {
                    // 密码验证通过，创建 JWT Token
                    var token = GenerateJwtToken(user);

                    // 返回成功登录的响应，包括 Token
                    return Ok(new { token });
                }

                return BadRequest("Invalid username or password");
            }

            return BadRequest("请等待管理审核。");
        }

        private string GenerateJwtToken(server.Mysql.Models.User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration["JwtSettings:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.ID.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30), // 设置 Token 过期时间
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    // 用户登录请求模型
    public class LogInRequest
    {
        [Required(ErrorMessage = "Account is required.")]
        [RegularExpression("^[a-zA-Z0-9]{1,20}$", ErrorMessage = "账号只能是20位以内的数字或者字母.")]
        public string Account { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression("^[a-zA-Z0-9!@#$%^&*()_+\\-=[\\]{};:'\\\",<.>/?]+$",
            ErrorMessage =
                "Password must be alphanumeric and may include special characters. It should be less than or equal to 30 characters.")]
        public string Password { get; set; }
    }
}