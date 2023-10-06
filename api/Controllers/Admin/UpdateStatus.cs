using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using server.Mysql.Data; // 导入数据注解命名空间

// 导入数据注解命名空间

namespace server.Controllers.Admin;

public partial class Api
{
    [FromServices] public MysqlDbContext DbContext { get; set; }


    // 修改用户状态
    [HttpPut("UpdateStatus")]
    public IActionResult UpdateStatus([FromBody] UpdateStatusRequest request)
    {
        int AdminId = GetUserIdByAccount(request.Account);

        if (AdminId != -1)
        {
            bool validIdentity = CheckUserIdentity(AdminId);

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
                    return Ok("Status updated successfully.");
                }
                else
                {
                    return NotFound("User not found.");
                }
            }

            return Unauthorized("Can't find your identity.");
        }

        return Unauthorized("Admin does not exist.");
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