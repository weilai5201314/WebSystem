﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using server.Mysql.Data;
using server.Mysql.Models;
using server.Time;

namespace server.Controllers.Admin
{
    public partial class Api
    {
        [FromServices] public MysqlDbContext DeleteUserContext { get; set; }

        // 使用 HTTP DELETE 请求删除用户账户
        [HttpDelete("DeleteUser")]
        public IActionResult DeleteUser([FromBody] DeleteUserRequest request)
        {
            // 判断管理员权限能否删除
            if (IsAdminAuthorizedToDelete(request.AdminAccount, request.UserAccount))
            {
                // 判断是否删除
                if (DeleteUserAccount(request.UserAccount))
                {
                    // 用户账户删除成功
                    LogDeleteUser(request.AdminAccount, true, request.UserAccount,
                        "User account deleted successfully.");
                    return Ok("User account deleted successfully.");
                }
                else
                {
                    // 用户账户删除失败
                    LogDeleteUser(request.AdminAccount, false, request.UserAccount, "Failed to delete user account.");
                    return BadRequest("Failed to delete user account.");
                }
            }
            else
            {
                // 管理员权限不足
                LogDeleteUser(request.AdminAccount, false, request.UserAccount, "Admin authorization failed.");
                return Unauthorized("Admin authorization failed.");
            }
        }

        // 日志函数
        private void LogDeleteUser(string adminAccount, bool success, string inputvalue, string returnvalue)
        {
            DateTime cstTime = TimeHelper.BeijingTime;

            // 创建日志实体
            var log = new Log
            {
                Timestamp = cstTime,
                User = adminAccount, // 记录管理员账号
                Action = "DeleteUser", // 记录操作名称
                InputResult = success,
                InputValue = $"AdminAccount: {adminAccount}, UserAccount: {inputvalue}",
                ReturnResult = success,
                ReturnValue = returnvalue // 这里可以记录操作成功的消息或返回的数据
            };
            // 将日志实体添加到数据库中
            DeleteUserContext.Log.Add(log);
            DeleteUserContext.SaveChanges();
        }


        // 删除用户账户的方法
        // 实现删除用户账户的逻辑，例如从数据库中删除用户信息
        // 如果成功，返回 true，否则返回 false
        private bool DeleteUserAccount(string userAccount)
        {
            try
            {
                var userToDelete = DeleteUserContext.User.FirstOrDefault(u => u.Account == userAccount);

                if (userToDelete != null)
                {
                    DeleteUserContext.User.Remove(userToDelete);
                    DeleteUserContext.SaveChanges();
                    return true; // 用户账户删除成功
                }
                else
                {
                    LogDeleteUser(userAccount, false, "当前函数：DeleteUserAccount", "待删除用户并不存在");
                    return false; // 用户账户不存在，删除失败
                }
            }
            catch (Exception ex)
            {
                // 处理异常，例如日志记录或返回适当的错误消息
                // 在实际应用中，你应该更详细地处理异常情况
                Console.WriteLine($"Error deleting user account: {ex.Message}");
                return false; // 删除过程中出现异常，删除失败
            }
        }


        // 判断管理员是否有权限删除用户账户的方法
        // 实现判断管理员权限的逻辑，根据你的业务规则进行验证
        // 如果管理员有权限，返回 true，否则返回 false
        // 同时要注意下级管理不能删除上级管理
        private bool IsAdminAuthorizedToDelete(string adminAccount, string userAccount)
        {
            // 获取管理员ID
            int adminID = GetUserIdByAccount(adminAccount);
            if (adminID != -1)
            {
                //  开始获取被删除用户ID
                // int userID = GetUserIdByAccount(userAccount);

                //  开始获取两个用户的权限，判断是否能删除
                //  身份组只有3 和 5 才能删除，且3不能删除5
                // bool result1 = CheckAdmin3_Identity(adminID);
                // bool result2 = CheckAdmin5_Identity(userID);
                // bool result2 = new Check(DeleteUserContext).CheckAdmin5_Identity(userID);
                // if (result1 && !result2)
                return CheckAdmin3_Identity(adminID);

                //  下级尝试删除上级
                return false;
            }

            // 不存在此 ID
            return false;
        }

        // 检测5管理员身份组
        // 需要用户的ID
        private bool CheckAdmin5_Identity(int adminId)
        {
            // 在这里查询数据库以验证用户的身份
            // 假设 UserUserGroup 表包含用户的身份信息
            var userUserGroupId = ShowinfoContext.UserUsergroup.Where(ug => ug.UserID == adminId);

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