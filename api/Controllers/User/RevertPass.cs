using Microsoft.AspNetCore.Mvc;
using server.HashEncry;
using server.Mysql.Data;
using System.ComponentModel.DataAnnotations; // 导入数据注解命名空间
using server.Mysql.Models;
using server.Time; // 导入Log实体类所在的命名空间

namespace server.Controllers.User;

public partial class Api
{
    [FromServices] public MysqlDbContext RevertPass_Context { get; set; }

    [HttpPost("RevertPass")]
    public IActionResult RevertPass([FromBody] RevertPassRequest request)
    {
        // 检测非法输入
        var validationContext = new ValidationContext(request, serviceProvider: null, items: null);
        var validationResults = new List<ValidationResult>();
        if (!Validator.TryValidateObject(request, validationContext, validationResults,
                validateAllProperties: true))
        {
            // 如果验证失败，返回错误消息
            LogRevertPass(request.Account, false, $"{request.Account} {request.Password}", "提交成功，等待管理审核。");
            return BadRequest(validationResults.Select(r => r.ErrorMessage));
        }

        // 判断用户是否存在
        // 根据用户名从数据库中查找用户
        var user = RevertPass_Context.User.FirstOrDefault(u => u.Account == request.Account);

        // 检查用户是否存在
        if (user == null)
        {
            // 记录提交找回密码失败的日志
            LogRevertPass(request.Account, false, $"{request.Account} {request.Password}", "提交成功，等待管理审核。");
            return BadRequest("Invalid username or password");
        }

        // 使用 PasswordHelper 类的 HashPassword 方法生成哈希密码
        var hash = PasswordHelper.HashPassword(request.Password, user.Salt);
        user.RevertPass = hash;
        user.Status = 3; // 3 待审核
        RevertPass_Context.SaveChanges();

        // 记录提交找回密码成功的日志
        LogRevertPass(request.Account, true, $"{request.Account} {request.Password}", "提交成功，等待管理审核。");

        return Ok("提交成功，等待管理审核。");
    }

// 添加记录提交找回密码操作的日志函数
    private void LogRevertPass(string userName, bool success, string inputvalue, string returnvalue)
    {
        DateTime cstTime = TimeHelper.BeijingTime;
        // 创建日志实体
        var log = new Log
        {
            Timestamp = cstTime,
            User = userName,
            Action = "RevertPass",
            InputResult = success,
            InputValue = $"UserName: {userName}, Input: {inputvalue}",
            ReturnResult = success,
            ReturnValue = returnvalue // 这里可以记录提交找回密码成功的消息或 Token
        };

        // 将日志实体添加到数据库中
        RevertPass_Context.Log.Add(log);
        RevertPass_Context.SaveChanges();
    }


    // 找回密码请求格式
    public class RevertPassRequest
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