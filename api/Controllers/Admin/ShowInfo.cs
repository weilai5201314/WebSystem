using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using server.Mysql.Data; // 导入数据注解命名空间

// 导入数据注解命名空间

namespace server.Controllers.Admin;

public partial class Api
{
    // 构造注入
    [FromServices] public MysqlDbContext ShowinfoContext { get; set; }


    [HttpPost("ShowInfo")]
    public IActionResult ShowInfo([FromBody] ShowInfoRequest request)
    {
        // string account = request.Account;

        // 在这里查询用户的ID
        int userId = GetUserIdByAccount(request.Account);

        if (userId != -1)
        {
            // 根据用户ID查询用户的身份
            bool validIdentity = CheckUserIdentity(userId);

            if (validIdentity)
            {
                // 用户有有效的身份，可以执行后续功能
                // 逻辑判断的正式通过，ShowInfo页面的所有业务功能从这开始/逻辑判断的正式通过，ShowInfo页面的所有业务功能从这开始/逻辑判断的正式通过，ShowInfo页面的所有业务功能从这开始/逻辑判断的正式通过，ShowInfo页面的所有业务功能从这开始

                // 开始查看user表的所有信息
                var userinfo = ShowinfoContext.User.Select(u => new
                {
                    u.Account,
                    u.Status
                }).ToList();

                // 返回所有user信息
                return Ok(userinfo);
                // return Ok("User has a valid identity. Proceed with the functionality.");
            }

            return Unauthorized("Can't find your identity.");
        }

        return Unauthorized("User does not have a valid identity or the account is invalid.");
    }

    // 通过账号获取ID 从User表
    private int GetUserIdByAccount(string account)
    {
        // 在这里查询 user 表以获取用户的ID
        var user = ShowinfoContext.User.FirstOrDefault(u => u.Account == account);

        if (user != null)
        {
            return user.ID;
        }

        return -1; // 返回-1表示未找到用户或账号无效
    }

    // 检测管理员身份组
    // 需要用户的ID
    private bool CheckUserIdentity(int userId)
    {
        // 在这里查询数据库以验证用户的身份
        // 假设 UserUserGroup 表包含用户的身份信息
        var UserUserGroupId = ShowinfoContext.UserUsergroup.Where(ug => ug.UserID == userId);

        foreach (var user in UserUserGroupId)
        {
            // 身份ID为3或5表示有效的身份
            if (user.UserGroupID == 3 || user.UserGroupID == 5)
            {
                return true;
            }
        }

        return false;
    }


    // 请求参数格式
    public class ShowInfoRequest
    {
        [Required(ErrorMessage = "Account is required.")]
        [RegularExpression("^[a-zA-Z0-9]{1,20}$", ErrorMessage = "账号只能是20位以内的数字或者字母.")]
        public string Account { get; set; }
    }
}


// curl -X POST "http://localhost:5009/Api/admin/ShowInfo" -H "Authorization: Bearer <your_jwt_token_here>" -v