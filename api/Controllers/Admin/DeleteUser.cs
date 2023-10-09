using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using server.Mysql.Data;
using server.Mysql.Models;

namespace server.Controllers.Admin
{
    public partial class Api
    {
        // 使用 HTTP DELETE 请求删除用户账户
        [HttpDelete("DeleteUser")]
        public IActionResult DeleteUser([FromBody] DeleteUserRequest request)
        {
            // 在这里执行删除用户账户的逻辑，根据需要调用数据库操作等
            // 你可以使用 request.AdminAccount 和 request.UserAccount 来获取管理员账号和要删除的用户账号

            // 以下是一个示例，你可以根据实际需求进行更改
            if (IsAdminAuthorizedToDelete(request.AdminAccount))
            {
                if (DeleteUserAccount(request.UserAccount))
                {
                    // 用户账户删除成功
                    return Ok("User account deleted successfully.");
                }
                else
                {
                    // 用户账户删除失败
                    return BadRequest("Failed to delete user account.");
                }
            }
            else
            {
                // 管理员权限不足
                return Unauthorized("Admin authorization failed.");
            }
        }

        // 判断管理员是否有权限删除用户账户的方法
        private bool IsAdminAuthorizedToDelete(string adminAccount)
        {
            // 实现判断管理员权限的逻辑，根据你的业务规则进行验证
            // 如果管理员有权限，返回 true，否则返回 false
            // 这里只是示例，你需要根据实际需求来实现权限验证
            return true;
        }

        // 删除用户账户的方法
        private bool DeleteUserAccount(string userAccount)
        {
            // 实现删除用户账户的逻辑，例如从数据库中删除用户信息
            // 如果成功，返回 true，否则返回 false
            // 这里只是示例，你需要根据实际需求来实现删除逻辑
            return true;
        }

        public class DeleteUserRequest
        {
            [Required(ErrorMessage = "AdminAccount is required.")]
            [RegularExpression("^[a-zA-Z0-9]{1,20}$", ErrorMessage = "AdminAccount只能是20位以内的数字或字母.")]
            public string AdminAccount { get; set; } // 管理员账号

            [Required(ErrorMessage = "UserAccount is required.")]
            [RegularExpression("^[a-zA-Z0-9]{1,20}$", ErrorMessage = "UserAccount只能是20位以内的数字或字母.")]
            public string UserAccount { get; set; } // 用户账号
        }
    }
}
