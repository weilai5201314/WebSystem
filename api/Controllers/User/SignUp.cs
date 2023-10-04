using Microsoft.AspNetCore.Mvc;
using server.HashEncry;
using server.Mysql.Data; // 导入数据库上下文

namespace server.Controllers.User
{
    public partial class Api
    {
        private readonly ApplicationDbContext _context; // 注入数据库上下文

        public Api(ApplicationDbContext context)
        {
            _context = context; // 注入数据库上下文
        }

        [HttpPost("SignUp")]
        public IActionResult SignUp([FromBody] SignUpRequest request)
        {
            if (_context.User.Any(u => u.Account == request.Username))
            {
                // 用户已存在，返回错误消息
                return BadRequest("Username already exists.");
            }

            // 使用 PasswordHelper 类的 GenerateSaltAndHash 方法生成盐值和哈希密码
            var (salt, hash) = PasswordHelper.GenerateSaltAndHash(request.Password);

            // 创建一个新的用户实体
            var newUser = new server.Mysql.Models.User
            {
                Account = request.Username,
                Pass = hash, // 存储哈希密码
                Salt = salt, // 存储盐值
                Status = 0
            };

            // 将新用户添加到数据库
            _context.User.Add(newUser);
            _context.SaveChanges();

            // 返回一个成功注册的 Ok 响应
            return Ok("User registered successfully");
        }
    }

    // 用户注册请求模型
    public class SignUpRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
