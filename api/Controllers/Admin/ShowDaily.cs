using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using server.Mysql.Data; // 导入数据注解命名空间
using server.Mysql.Models;
using server.Time; // 导入Log实体类所在的命名空间

namespace server.Controllers.Admin;

public partial class Api
{
    [FromServices] public MysqlDbContext ShowDailyContext { get; set; }

    [HttpPost("ShowDaily")]
    public IActionResult ShowDaily([FromBody] ShowDailyRequest request)
    {
        // 在这里查询用户的ID
        int userId = GetUserIdByAccount(request.Account);

        if (userId != -1)
        {
            // 根据用户ID查询用户的身份
            bool validIdentity = CheckDailyIdentity(userId);

            if (validIdentity)
            {
                // 逻辑判断的正式通过，ShowDaily页面的所有业务功能从这开始

                // 开始查看Log表的所有信息
                var loginfo = ShowinfoContext.Log.Select(u => new
                {
                    u.ID,
                    u.Timestamp,
                    u.User,
                    u.Action,
                    u.InputResult,
                    u.InputValue,
                    u.ReturnResult,
                    u.ReturnValue
                }).ToList();
                
                // 这里判断是否读取日志成功不是很好，因为loginfo.cout实际上是读取的日志数
                bool succes = loginfo.Count > 0;
                // 记录操作日志
                LogShowDaily(request.Account, succes, request.Account, "ShowAll daily.");
                // 返回所有Log信息
                return Ok(loginfo);
            }

            // 记录操作失败的日志
            LogShowDaily(request.Account, false, request.Account, "Can't find your identity.");
            return Unauthorized("Can't find your identity.");
        }

        LogShowDaily(request.Account, false, request.Account, "No such Admin.");
        return Ok("No such Admin.");
    }

    // 添加记录操作日志的函数
    private void LogShowDaily(string userName, bool success, string inputvalue, string returnvalue)
    {
        DateTime cstTime = TimeHelper.BeijingTime;

        // 创建日志实体
        var log = new Log
        {
            Timestamp = cstTime,
            User = userName,
            Action = "ShowDaily",
            InputResult = success,
            InputValue = $"Input: {inputvalue}",
            ReturnResult = success,
            ReturnValue = returnvalue // 这里可以记录操作成功的消息或返回的数据
        };

        // 将日志实体添加到数据库中
        ShowinfoContext.Log.Add(log);
        ShowinfoContext.SaveChanges();
    }

    // 检测日志管理员身份
    private bool CheckDailyIdentity(int userId)
    {
        // 在这里查询数据库以验证用户的身份
        // 假设 UserUserGroup 表包含用户的身份信息
        var UserUserGroupId = ShowDailyContext.UserUsergroup.Where(ug => ug.UserID == userId);

        foreach (var user in UserUserGroupId)
        {
            // 身份ID为4表示有 日志管理员
            if (user.UserGroupID == 4 || user.UserGroupID == 5)
            {
                return true;
            }
        }

        return false;
    }

// 请求参数格式
    public class ShowDailyRequest
    {
        [Required(ErrorMessage = "Account is required.")]
        [RegularExpression("^[a-zA-Z0-9]{1,20}$", ErrorMessage = "账号只能是20位以内的数字或者字母.")]
        public string Account { get; set; }
    }
}