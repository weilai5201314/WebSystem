using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace server.Controllers
{
    public partial class Api
    {
        [HttpPost("user/SignUp2")]
        public IActionResult SignUp2([FromBody] SignUp2Request request)
        {
            // 判断用户是否已存在
            if (!(SignUp_context.User.Any(u => u.Account == request.Account)))
            {
                return BadRequest("User not found.");
            }
            var user = DbContext.User.FirstOrDefault(u => u.Account == request.Account);
            //  提取 N+1 次迭代值入库
            //  先转化，再入库
            try
            {
                user.Pass = Convert.FromBase64String(request.Password);
            }
            catch (FormatException)
            {
                return BadRequest("Invalid Base64 string for password.");
            }
            //  开始保存
            SignUp_context.User.Update(user);
            SignUp_context.SaveChanges();

            TypeLog(request.Account, "Signup", true, $"{request.Account} ", false, "注册成功，等待管理审核。");
            return Ok("注册成功，等待管理审核。");
        }
    }

    public class SignUp2Request
    {
        [Required(ErrorMessage = "Account is required.")]
        [RegularExpression("^[a-zA-Z0-9]{1,20}$", ErrorMessage = "账号只能是20位以内的数字或者字母.")]
        public string Account { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        // [RegularExpression("^[a-zA-Z0-9]{8,20}$", ErrorMessage = "密码只能是8-20位的数字或者字母.")]
        public string Password { get; set; }
    }
}