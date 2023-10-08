using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using server.Mysql.Data;
using server.Mysql.Models; // 导入数据注解命名空间

// 导入数据注解命名空间

namespace server.Controllers.Admin;

public partial class Api
{
    [HttpPut("UpdateIdentity")]
    public IActionResult UpdateIdentity([FromBody] UpdateIdentityRequest request)
    {
        // 判断是否有行为
        if (request.HasAction)
        {
            if (2 <= request.IdentityId && request.IdentityId <= 5)
            {
                string adminAccount = request.AdminAccount;
                string userAccount = request.UserAccount;
                int identityId = request.IdentityId;
                int action = request.Action;
                //判断AdminID 是否有效
                int AdminID = GetUserIdByAccount(adminAccount);
                if (AdminID != -1)
                {
                    // 开始判断管理员权限
                    bool validIdentity = CheckUserIdentity(AdminID);
                    if (validIdentity)
                    {
                        // 从user表先获取ID
                        int userId = GetUserIdByAccount(userAccount);
                        // 增加
                        // 在 UpdateIdentity 方法中
                        if (action == 1)
                        {
                            var result = AddIdentityToUser(userId, identityId);
                            var success = result is OkObjectResult;
                            string logMessage = success ? "Identity added successfully." : "Failed to add identity.";
                            LogUpdateIdentity(request.AdminAccount, request.UserAccount, identityId, action, success,
                                logMessage);
                            return result;
                        }
                        else if (action == 2)
                        {
                            var result = RemoveIdentityFromUser(userId, identityId);
                            var success = result is OkObjectResult;
                            string logMessage = success ? "Identity added successfully." : "Failed to add identity.";
                            LogUpdateIdentity(request.AdminAccount, request.UserAccount, identityId, action, success,
                                logMessage);
                            return result;
                        }
                    }

                    LogUpdateIdentity(request.AdminAccount, request.UserAccount, identityId, action, false,
                        "Can't find your identity.");
                    return Unauthorized("Can't find your identity.");
                }

                LogUpdateIdentity(request.AdminAccount, request.UserAccount, identityId, action, false,
                    "No such Admin.");
                return Ok("No such Admin.");
            }

            LogUpdateIdentity(request.AdminAccount, request.UserAccount, request.IdentityId, request.Action, false,
                "No such identity.");
            return Ok("No such identity.");
        }


        LogUpdateIdentity(request.AdminAccount, request.UserAccount, request.IdentityId, request.Action, false,
            "No action specified.");
        return Ok("No action specified.");
    }

    // 添加记录操作日志的函数
    // 修改 LogUpdateIdentity 函数，接收 IActionResult 作为参数
    private void LogUpdateIdentity(string adminAccount, string userAccount, int identityId, int action,
        bool success, string message)
    {
        // 创建日志实体
        var log = new Log
        {
            Timestamp = DateTime.UtcNow,
            User = adminAccount, // 记录管理员账号
            Action = "UpdateIdentity", // 记录操作名称
            InputResult = success,
            InputValue =
                $"AdminAccount: {adminAccount}, UserAccount: {userAccount}, IdentityId: {identityId}, Action: {action}",
            ReturnResult = success,
            ReturnValue = message // 这里可以记录操作成功的消息或返回的数据
        };

        // 将日志实体添加到数据库中
        ShowinfoContext.Log.Add(log);
        ShowinfoContext.SaveChanges();
    }


    // 添加身份
    private IActionResult AddIdentityToUser(int userId, int identityId)
    {
        // 实现添加身份的逻辑，例如向数据库中添加身份信息
        // 如果成功，返回 true，否则返回 false
        // / 检查用户是否已关联要添加的身份组
        bool userHasIdentity = ShowinfoContext.UserUsergroup
            .Any(ug => ug.UserID == userId && ug.UserGroupID == identityId);

        if (!userHasIdentity)
        {
            // 用户没有关联要添加的身份组，执行添加操作
            var newUserUsergroup = new UserUsergroup
            {
                UserID = userId,
                UserGroupID = identityId
            };

            ShowinfoContext.UserUsergroup.Add(newUserUsergroup);
            ShowinfoContext.SaveChanges();

            return Ok("Identity added successfully.");
        }

        return BadRequest("User already has this identity.");
    }

    // 删除身份
    private IActionResult RemoveIdentityFromUser(int userId, int identityId)
    {
        // 实现删除身份的逻辑，例如从数据库中删除身份信息
        // 如果成功，返回 true，否则返回 false
        // 检查用户是否已关联要删除的身份组
        var userUsergroup = ShowinfoContext.UserUsergroup
            .FirstOrDefault(ug => ug.UserID == userId && ug.UserGroupID == identityId);

        if (userUsergroup != null)
        {
            // 用户已关联要删除的身份组，执行删除操作
            ShowinfoContext.UserUsergroup.Remove(userUsergroup);
            ShowinfoContext.SaveChanges();
            return Ok("Identity removed successfully.");
        }

        return BadRequest("User does not have this identity.");
    }


    public class UpdateIdentityRequest
    {
        public string AdminAccount { get; set; } // 管理员账号
        public string UserAccount { get; set; } // 用户账号
        public int IdentityId { get; set; } // 身份ID
        public int Action { get; set; } = 0; // 默认为 0，表示不执行任何操作
        // 1为添加，2为删除

        // 可选参数 Action
        public bool HasAction => Action != 0; //判断是否有参数
    }
}