using Microsoft.AspNetCore.Mvc;
using server.Controllers.File;

namespace server.Controllers;

public partial class Api
{
    [HttpPost("File/DeleteFile")]
    public IActionResult DeleteFile([FromBody] FileAccessRequest request)
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
                TypeLog(request.UserName, "DeleteFile", true, $"FileName:{request.ObjectName1}", false,
                    "Invalid username");
                return BadRequest("Invalid username");
            }


            // 验证文件名是否已存在
            if (!FileExists(request.ObjectName1))
            {
                TypeLog(request.UserName, "DeleteFile", true, $"FileName:{request.ObjectName1}", false,
                    "File isn't exists");
                return Ok($"File '{request.ObjectName1}' isn't exists");
            }


            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //---------------------------------------------------自主访问控制---------------------------------------------------//
            //  只有4权限能删除文件
            if (!HasPermission(user.ID, request.ObjectName1, 4))
            {
                TypeLog(request.UserName, "DeleteFile", true, $"FileName:{request.ObjectName1}", false,
                    "User does not have delete permission");
                return BadRequest("You do not have permission to delete this file");
            }

            //---------------------------------------------------自主访问控制---------------------------------------------------//
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //---------------------------------------------------强制访问控制---------------------------------------------------//
            //  目前还没开始写
            //---------------------------------------------------强制访问控制---------------------------------------------------//

            //  通过全部检测，开始删除文件
            int fileId = DbContext.Resource.Where(u => u.FileName == request.ObjectName1).Select(u => u.ID)
                .FirstOrDefault();

            // 删除数据库 文件记录和关联表信息
            if (DeleteFileAndRecords(user.ID, fileId, request.ObjectName1))
            {
                // 从磁盘删除文件
                System.IO.File.Delete(Path.Combine(DirectoryPath, request.ObjectName1));

                TypeLog(request.UserName, "DeleteFile", true, $"FileName:{request.ObjectName1}", true,
                    "File deleted successfully");
                return Ok($"File '{request.ObjectName1}' deleted successfully");
            }

            // 处理删除失败的情况
            TypeLog(request.UserName, "DeleteFile", true, $"FileName:{request.ObjectName1}", false,
                "File deletion failed");
            return BadRequest("File deletion failed");
        }
        catch (Exception ex)
        {
            // 记录日志或返回错误信息
            TypeLog(request.UserName, "DeleteFile", true, $"FileName:{request.ObjectName1}", false,
                $"Error adding file: {ex.Message}");
            return StatusCode(500, $"Error adding file: {ex.Message}");
        }
    }

    // 提取删除文件记录和关联表信息的方法
    private bool DeleteFileAndRecords(int userId, int fileId, string fileName)
    {
        // 从关联表中删除相关信息
        var permissionToDelete = DbContext.UserResourcePermission
            .FirstOrDefault(urp => urp.UserID == userId && urp.ResourceID == fileId);
        if (permissionToDelete == null)
        {
            TypeLog("System", "DeleteFile", true, $"FileName:{fileName}", false,
                "Failed to delete association table record");
            return false;
        }

        DbContext.UserResourcePermission.Remove(permissionToDelete);
        DbContext.SaveChanges();
        // 从 Resource 表中删除文件记录
        var fileToDelete = DbContext.Resource.Find(fileId);
        if (fileToDelete == null)
        {
            TypeLog("System", "DeleteFile", true, $"FileName:{fileName}", false,
                "Failed to delete resource table record");
            return false;
        }

        DbContext.Resource.Remove(fileToDelete);
        DbContext.SaveChanges();
        return true;
    }
}