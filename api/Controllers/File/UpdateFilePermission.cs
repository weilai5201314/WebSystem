using Microsoft.AspNetCore.Mvc;
using server.Controllers.File;
using server.Mysql.Models;

namespace server.Controllers;

public partial class Api
{
    [HttpPost("File/UpdateFilePermission")]
    public IActionResult UpdateFilePermission([FromBody] FilePermissionRequest request)
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
                TypeLog(request.UserName, "UpdateFilePermission", true, $"{request.UserName}", false,
                    "Invalid username");
                return BadRequest("Invalid username");
            }

            // 获取被修改权限的用户信息
            var alterUser = DbContext.User.FirstOrDefault(u => u.Account == request.AlterUserName);
            if (alterUser == null)
            {
                TypeLog(request.UserName, "UpdateFilePermission", true, $"{request.AlterUserName}", false,
                    "Invalid username");
                return BadRequest("Invalid username");
            }

            // 验证文件名是否存在
            if (!FileExists(request.FileName))
            {
                TypeLog(request.UserName, "UpdateFilePermission", true, $"FileName:{request.FileName}", false,
                    "File isn't exists");
                return Ok($"File '{request.FileName}' isn't exists");
            }

            var newFile = DbContext.Resource.FirstOrDefault(f => f.FileName == request.FileName);
            if (newFile == null)
                return BadRequest("File isn't exists");

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //---------------------------------------------------自主访问控制---------------------------------------------------//
            // 判断用户是否是文件拥有者，否则不能进行修改,4 权限为拥有权
            if (HasPermission(user.ID, request.FileName, 4))
            {
                // 根据请求action判断是增加还是删除权限,根据permission判断哪个权限
                // action 1 增加，2删除
                // permission 1读，2写
                if (request.Action == 1)
                    GrantPermission(alterUser.ID, newFile.ID, request.Permission);

                if (request.Action == 2)
                    DeletePermission(alterUser.ID, newFile.ID, request.Permission);
            }
            //---------------------------------------------------自主访问控制---------------------------------------------------//
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            TypeLog(request.UserName, "UpdateFilePermission", true, $"FileName:{request.FileName}", true,
                "File added successfully");
            return Ok($"File '{request.FileName}' alter ");
        }
        catch (Exception ex)
        {
            // 记录日志或返回错误信息
            return StatusCode(500, $"Error adding file: {ex.Message}");
        }
    }
    
    // 删除用户对文件的权限，返回是否删除成功
    // 内置Log，无需编写
    private bool DeletePermission(int ownerId, int fileId, int permissionCode)
    {
        try
        {
            // 查询Owner权限ID
            int ownerPermissionId = DbContext.Permission
                .Where(p => p.PermissionCode == permissionCode)
                .Select(p => p.ID)
                .FirstOrDefault(); // 防止空异常

            if (ownerPermissionId == 0)
            {
                // 如果未找到Owner权限ID，记录错误的日志并返回失败
                TypeLog("System", "DeleteOwnerPermission", true, "Error finding Owner permission ID", false,
                    "Error finding Owner permission ID");
                return false;
            }

            // 在关联表中删除Owner权限记录
            var existingFilePermission = DbContext.UserResourcePermission
                .FirstOrDefault(urp => urp.UserID == ownerId
                                       && urp.ResourceID == fileId
                                       && urp.PermissionID == ownerPermissionId);

            if (existingFilePermission != null)
            {
                DbContext.UserResourcePermission.Remove(existingFilePermission);
                DbContext.SaveChanges();

                TypeLog("System", "DeleteOwnerPermission", true, $"{ownerId} {fileId} {permissionCode}"
                    , true, $"Owner permission deleted successfully for file ID '{fileId}'");
                return true;
            }
            else
            {
                // 如果未找到要删除的记录，记录错误的日志并返回失败
                TypeLog("System", "DeleteOwnerPermission", true, $"Permission record not found for deletion", false,
                    $"Permission record not found for deletion");
                return false;
            }
        }
        catch (Exception ex)
        {
            // 记录错误的日志
            TypeLog("System", "DeleteOwnerPermission", false, $"Error deleting Owner permission: {ex.Message}",
                false, $"{ex.Message}");
            return false;
        }
    }
}