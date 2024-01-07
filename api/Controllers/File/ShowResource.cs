using System.Data;
using Microsoft.AspNetCore.Mvc;
using server.Controllers.File;
using MySql.Data.MySqlClient;

namespace server.Controllers;

public partial class Api
{
    private readonly IConfiguration _configuration;

    public Api(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }


    // 注入 IConfiguration


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

            // 如果 action 3，获取关联表信息，以拥有 6 权限 ID 的文件为筛选条件
            // if (request.Action == 3)
            // {
            //     // 获取当前用户 ID
            //     var currentUser = DbContext.User.FirstOrDefault(u => u.Account == request.UserName);
            //     if (currentUser != null)
            //     {
            //         int currentUserId = currentUser.ID;
            //
            //         // 执行查询
            //         var result = (from urp in DbContext.UserResourcePermission
            //             where urp.ResourceID == DbContext.UserResourcePermission
            //                       .Where(inner => inner.UserID == currentUserId && inner.PermissionID == 6)
            //                       .Select(inner => inner.ResourceID)
            //                       .FirstOrDefault()
            //                   && urp.UserID != currentUserId
            //             join u in DbContext.User on urp.UserID equals u.ID
            //             join r in DbContext.Resource on urp.ResourceID equals r.ID
            //             join p in DbContext.Permission on urp.PermissionID equals p.ID
            //             select new { u.Account, r.FileName, p.PermissionCode }).ToList();
            //
            //
            //         TypeLog(request.UserName, "ShowFile", true, $"Name:{request.UserName}",
            //             true, "action3.");
            //         return Ok(result);
            //     }
            // }
            // 如果 action 3，获取关联表信息，以拥有 6 权限 ID 的文件为筛选条件
            if (request.Action == 3)
            {
                try
                {
                    // 创建 MySQL 连接
                    using (MySqlConnection connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                    {
                        connection.Open();

                        // 创建 MySQL 命令
                        using (MySqlCommand command = new MySqlCommand())
                        {
                            command.Connection = connection;
                            command.CommandType = CommandType.Text;
                            command.CommandText = @"
                            SELECT u.Account, r.FileName, p.PermissionCode
                            FROM userresourcepermission urp
                            JOIN user u ON urp.UserID = u.ID
                            JOIN resource r ON urp.ResourceID = r.ID
                            JOIN permission p ON urp.PermissionID = p.ID
                            WHERE urp.ResourceID in (
                                SELECT ResourceID
                                FROM userresourcepermission
                                WHERE UserID = @UserId AND PermissionID = 6
                            ) AND urp.UserID != @UserId";

                            // 设置参数
                            command.Parameters.AddWithValue("@UserId", user.ID);

                            // 执行查询
                            using (MySqlDataReader reader = command.ExecuteReader())
                            {
                                List<object> result = new List<object>();
                                while (reader.Read())
                                {
                                    result.Add(new
                                    {
                                        Account = reader["Account"].ToString(),
                                        FileName = reader["FileName"].ToString(),
                                        PermissionCode = int.Parse(reader["PermissionCode"].ToString())
                                    });
                                }

                                return Ok(result);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    TypeLog(request.UserName, "ShowFile", true, $"Name:{request.UserName}", false,
                        $"Error show file: {ex.Message}");
                    return StatusCode(500, $"Error show file: {ex.Message}");
                }
            }


            // 展示最基础的用户列表
            if (request.Action == 4)
                fileInfo = DbContext.User.Select(i => i.Account).ToList();


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