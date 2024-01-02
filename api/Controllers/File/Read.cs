using Microsoft.AspNetCore.Mvc;
using server.Controllers.File;
using server.Mysql.Models;

namespace server.Controllers;

public partial class Api
{
    [HttpPost("File/ReadFile")]
    public IActionResult ReadFile([FromBody] FileAccessRequest request)
    {
        try
        {
            // 验证请求参数
            if (request == null)
                return BadRequest("Invalid request");

            // 获取当前用户信息
            var user = DbContext.User.FirstOrDefault(u => u.Account == request.UserName);
            if (user == null)
            {
                TypeLog(request.UserName, "ReadFile", true, $"FileName:{request.ObjectName1}", false,
                    "Invalid username");
                return BadRequest("Invalid username");
            }

            // 验证文件名是否已存在
            if (!FileExists(request.ObjectName1))
            {
                TypeLog(request.UserName, "ReadFile", true, $"FileName:{request.ObjectName1}", false,
                    "File already exists");
                return Ok($"File '{request.ObjectName1}' isn't exists");
            }

            //---------------------------------------------------自主访问控制---------------------------------------------------//
            //  判断用户能否读此文件，只有 1 读权限 或者 4 拥有权限 才能读取文件
            if (!HasPermission(user.ID, request.ObjectName1, 1) && !HasPermission(user.ID, request.ObjectName1, 4))
            {
                TypeLog(request.UserName, "ReadFile", true, $"FileName:{request.ObjectName1}", false,
                    "User does not have read permission");
                return BadRequest("You do not have permission to read this file");
            }

            //---------------------------------------------------自主访问控制---------------------------------------------------//

            //---------------------------------------------------强制访问控制---------------------------------------------------//
            //  目前还没开始写
            //---------------------------------------------------强制访问控制---------------------------------------------------//

            string filePath = Path.Combine("D:\\zzz\\school\\InfoSecurity\\WebSystem\\api\\Data", request.ObjectName1);
            string fileContent = System.IO.File.ReadAllText(filePath);
            TypeLog(request.UserName, "ReadFile", true, $"FileName:{request.ObjectName1}", true,
                "successfully read file");
            return Ok(fileContent);
        }
        catch (Exception ex)
        {
            // 记录日志或返回错误信息
            return StatusCode(500, $"Error adding file: {ex.Message}");
        }
    }

    // 判断用户是否具有指定文件的指定权限
    private bool HasPermission(int userId, string fileName, int permissionCode)
    {
        // 查询指定文件的指定权限ID
        int permissionId = DbContext.Permission
            .Where(p => p.PermissionCode == permissionCode)
            .Select(p => p.ID)
            .FirstOrDefault();

        if (permissionId == 0)
        {
            // 如果未找到权限ID，记录错误的日志并返回失败
            TypeLog("System", "HasPermission", true, "Error finding permission ID", false,
                "Error finding permission ID");
            return false;
        }

        //  获取文件ID
        int fileId = DbContext.Resource.Where(u => u.FileName == fileName).Select(u => u.ID).FirstOrDefault();


        // 检查用户是否具有指定文件的指定权限
        return DbContext.UserResourcePermission
            .Any(urp => urp.UserID == userId && urp.ResourceID == fileId && urp.PermissionID == permissionId);
    }
}