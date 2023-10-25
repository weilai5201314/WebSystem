using Microsoft.AspNetCore.Mvc;
using server.Mysql.Data;

namespace server.lib.admin;

public class Check
{
    [FromServices] public MysqlDbContext CheckUserContext { get; set; }

    // 检测5管理员身份组
    // 需要用户的ID
    public bool CheckAdmin5_Identity(int adminId)
    {
        // 在这里查询数据库以验证用户的身份
        // 假设 UserUserGroup 表包含用户的身份信息
        var userUserGroupId = CheckUserContext.UserUsergroup.Where(ug => ug.UserID == adminId);

        foreach (var user in userUserGroupId)
        {
            // 身份ID为3或5表示有效的身份
            if (user.UserGroupID == 5)
            {
                return true;
            }
        }

        return false;
    }
}