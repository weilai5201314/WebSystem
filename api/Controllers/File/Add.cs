using Microsoft.AspNetCore.Mvc;
using server.Controllers.File;
using server.Mysql.Models;

namespace server.Controllers;

public partial class Api
{
    [HttpPost("File/AddFile")]
    public IActionResult AddFile([FromBody] FileAccessRequest request)
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
                TypeLog(request.UserName, "AddFile", true, $"FileName:{request.ObjectName1}", false,
                    "Invalid username");
                return BadRequest("Invalid username");
            }

            //---------------------------------------------------强制访问控制---------------------------------------------------//
            //  判断用户是否有访问类来创建文件

            //---------------------------------------------------强制访问控制---------------------------------------------------//


            // 验证文件名是否已存在
            if (FileExists(request.ObjectName1))
            {
                TypeLog(request.UserName, "AddFile", true, $"FileName:{request.ObjectName1}", false,
                    "File already exists");
                return Ok($"File '{request.ObjectName1}' already exists");
            }

            // 创建文件
            var newFile = CreateFile(request.ObjectName1, request.Text);

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //---------------------------------------------------自主访问控制---------------------------------------------------//
            // 为文件拥有者添加Owner权限
            // 4 表示Owner权限
            bool result = GrantPermission(user.ID, newFile.ID, 4);
            //---------------------------------------------------自主访问控制---------------------------------------------------//
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            TypeLog(request.UserName, "AddFile", true, $"FileName:{request.ObjectName1}", true,
                "File added successfully");
            return Ok($"File '{request.ObjectName1}' added successfully");
        }
        catch (Exception ex)
        {
            // 记录日志或返回错误信息
            return StatusCode(500, $"Error adding file: {ex.Message}");
        }
    }


    //  检查文件是否存在
    //  内部没有Log函数，自行添加
    private bool FileExists(string fileName)
    {
        // 根据文件名查询文件表，判断文件是否已存在

        return DbContext.Resource.Any(f => f.FileName == fileName);
    }

    //  在磁盘创建文件，并保存文件名进数据库
    //  无Log，自行编写
    private Resource CreateFile(string fileName, string fileText)
    {
        // 创建文件，保存到磁盘上
        string filePath = Path.Combine(DirectoryPath, fileName);
        System.IO.File.WriteAllText(filePath, fileText);

        var newFile = new Resource
        {
            FileName = fileName
            // 其他文件属性的设置，如文件类型、创建时间等
        };

        // 添加到数据库
        DbContext.Resource.Add(newFile);
        DbContext.SaveChanges();

        return newFile;
    }


    //  为用户和文件添加权限，返回是否添加成功
    //  内置Log，无需编写
    private bool GrantPermission(int ownerId, int fileId, int permissionCode)
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
                TypeLog("System", "GrantOwnerPermission", true, "Error finding Owner permission ID", false,
                    "Error finding Owner permission ID");
                return false;
            }

            // 查询是否已经存在此权限
            bool permissionExists = DbContext.UserResourcePermission
                .Any(urp => urp.UserID == ownerId && urp.ResourceID == fileId && urp.PermissionID == ownerPermissionId);

            if (permissionExists)
            {
                // 如果权限已经存在，记录日志并返回失败
                TypeLog("System", "GrantOwnerPermission", true,
                    $"Permission already exists for user '{ownerId}' and file '{fileId}'", false,
                    $"Permission already exists for user '{ownerId}' and file '{fileId}'");
                return false;
            }

            // 在关联表中添加Owner权限记录
            var newFilePermission = new UserResourcePermission
            {
                UserID = ownerId,
                ResourceID = fileId,
                PermissionID = ownerPermissionId
            };

            DbContext.UserResourcePermission.Add(newFilePermission);
            DbContext.SaveChanges();

            TypeLog("System", "GrantOwnerPermission", true, $"{ownerId} {fileId} {permissionCode}"
                , true, $"Owner permission granted successfully for file ID '{fileId}'");
            return true;
        }
        catch (Exception ex)
        {
            // 记录错误的日志
            TypeLog("System", "GrantOwnerPermission", false, $"Error granting Owner permission: {ex.Message}",
                false, $"{ex.Message}");
            return false;
        }
    }
}