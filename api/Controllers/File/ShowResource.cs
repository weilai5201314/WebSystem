using Microsoft.AspNetCore.Mvc;
using server.Controllers.File;

namespace server.Controllers;

public partial class Api
{
    [HttpPost("File/ShowFile")]
    public IActionResult ShowFile([FromBody] FileAccessRequest request)
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
                TypeLog(request.UserName, "ShowFile", true, $"{request.UserName}", false,
                    "Invalid username");
                return BadRequest("Invalid username");
            }

            //  开始获取文件列表
            List<string>? fileInfo = null;
            //  如果  action 1,则直接获取所有文件列表
            if (request.Action == 1)
                fileInfo = DbContext.Resource.Select(u => u.FileName).ToList();
            //  如果  action 2,则根据用户ID来获取权限为 4 的文件名
            if (request.Action == 2)
            {
                // 获取用户ID为指定用户（request.UserId）并且权限为4的文件名,注意权限为4对应的ID
                fileInfo = DbContext.UserResourcePermission
                    .Where(urp => urp.UserID == user.ID && urp.PermissionID == 6)
                    .Join(DbContext.Resource,
                        urp => urp.ResourceID,
                        resource => resource.ID,
                        (urp, resource) => resource.FileName)
                    .ToList();
            }

            //  如果  action 3,获取关联表信息，以拥有4权限的文件为筛选条件
            // if (request.Action == 3)
            // {
            //     // 获取当前用户拥有的所有文件
            //     var userFiles = DbContext.UserResourcePermission
            //         .Where(urp => urp.UserID == user.ID)
            //         .Join(DbContext.Resource,
            //             urp => urp.ResourceID,
            //             resource => resource.ID,
            //             (urp, resource) => new
            //             {
            //                 UserAccount = user.Account,
            //                 FileName = resource.FileName
            //             })
            //         .ToList();
            //
            //     // 获取其他用户对这些文件的权限信息
            //     var otherUsersPermissions = DbContext.UserResourcePermission
            //         .Where(urp => userFiles.Select(uf => uf.FileName).Contains(urp.Resource.FileName) && urp.UserID != user.ID)
            //         .Join(DbContext.User,
            //             urp => urp.UserID,
            //             otherUser => otherUser.ID,
            //             (urp, otherUser) => new
            //             {
            //                 UserAccount = otherUser.Account,
            //                 FileName = urp.Resource.FileName,
            //                 PermissionDescription = urp.Permission.PermissionDescription
            //             })
            //         .ToList();
            //
            //     // 将结果赋值给 fileInfo，这里 fileInfo 的类型需要调整为 List<你的结果类的类型>
            //     fileInfo = otherUsersPermissions.Select(r => new YourResultClass
            //     {
            //         UserAccount = r.UserAccount,
            //         FileName = r.FileName,
            //         PermissionDescription = r.PermissionDescription
            //     }).ToList();
            // }



            bool successful = fileInfo is { Count: > 0 };
            TypeLog(request.UserName, "ShowFile", true, $"Name:{request.UserName}",
                successful, "Show All file name.");

            return Ok(fileInfo);
        }
        catch (Exception ex)
        {
            // 记录日志或返回错误信息
            TypeLog(request.UserName, "ShowFile", true, $"Name:{request.UserName}",
                false, $"Error show file: {ex.Message}");
            return StatusCode(500, $"Error show file: {ex.Message}");
        }
    }
}