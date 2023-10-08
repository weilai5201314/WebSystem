using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using server.Mysql.Data;
using server.Mysql.Models; // 导入数据注解命名空间

// 导入数据注解命名空间

namespace server.Controllers.Admin;

public partial class Api
{
    [FromServices] public MysqlDbContext DbContext { get; set; }


    // 修改用户状态
    [HttpPut("UpdateStatus")]
    public IActionResult UpdateStatus([FromBody] UpdateStatusRequest request)
    {
        int adminId = GetUserIdByAccount(request.Account);

        if (adminId != -1)
        {
            bool validIdentity = CheckUserIdentity(adminId);

            if (validIdentity)
            {
                // 获取被修改用户ID
                int userId = GetUserIdByAccount(request.AlterAccount);
                // 执行修改状态的操作，将 request.NewStatus 更新到数据库中
                var user = DbContext.User.FirstOrDefault(u => u.ID == userId);
                if (user != null)
                {
                    user.Status = request.NewStatus;
                    DbContext.SaveChanges();

                    // 调用日志函数记录成功的操作
                    LogStatus(request.Account, request.AlterAccount, request.NewStatus, true,
                        "Status updated successfully.");

                    return Ok("Status updated successfully.");
                }
                else
                {
                    // 修改用户没找到
                    LogStatus(request.Account, request.AlterAccount, request.NewStatus,
                        false, "User not found.");
                    return NotFound("User not found.");
                }
            }

            // 权限不够
            LogStatus(request.Account, request.AlterAccount, request.NewStatus,
                false, "Can't find your identity.");
            return Unauthorized("Can't find your identity.");
        }

        // 不存此id
        LogStatus(request.Account, request.AlterAccount, request.NewStatus,
            false, "Admin does not exist.");
        return Unauthorized("Admin does not exist.");
    }

    // 添加记录操作日志的函数
    private void LogStatus(string adminAccount, string userAccount, int status, bool success, string message)
    {
        // 创建日志实体
        var log = new Log
        {
            Timestamp = DateTime.UtcNow,
            User = adminAccount, // 记录管理员账号
            Action = "UpdateStatus", // 记录操作名称
            InputResult = success,
            InputValue = $"AdminAccount: {adminAccount}, UserAccount: {userAccount},Newstatus:{status}",
            ReturnResult = success,
            ReturnValue = message // 这里可以记录操作成功的消息或返回的数据
        };

        // 将日志实体添加到数据库中
        DbContext.Log.Add(log);
        DbContext.SaveChanges();
    }

    // 请求格式
    public class UpdateStatusRequest
    {
        [Required(ErrorMessage = "Account is required.")]
        [RegularExpression("^[a-zA-Z0-9]{1,20}$", ErrorMessage = "账号只能是20位以内的数字或者字母.")]
        public string Account { get; set; }

        [Required(ErrorMessage = "Account is required.")]
        [RegularExpression("^[a-zA-Z0-9]{1,20}$", ErrorMessage = "账号只能是20位以内的数字或者字母.")]
        public string AlterAccount { get; set; }

        [Required(ErrorMessage = "NewStatus is required.")]
        public int NewStatus { get; set; }
    }
}