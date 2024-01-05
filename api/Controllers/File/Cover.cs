using Microsoft.AspNetCore.Mvc;
using server.Controllers.File;

namespace server.Controllers;

public partial class Api
{
    [HttpPost("File/CoverFile")]
    public IActionResult Cover([FromBody] FileAccessRequest request)
    {
        // 处理写文件的逻辑
        try
        {
            // 验证请求参数
            if (request == null)
                return BadRequest("Invalid request");

            // 获取当前用户信息
            var user = DbContext.User.FirstOrDefault(u => u.Account == request.UserName);
            if (user == null)
            {
                TypeLog(request.UserName, "CoverFile", true, $"FileName:{request.ObjectName1}", false,
                    "Invalid username");
                return BadRequest("Invalid username");
            }

            // 验证文件名是否已存在
            if (!FileExists(request.ObjectName1))
            {
                TypeLog(request.UserName, "CoverFile", true, $"FileName:{request.ObjectName1}", false,
                    "File already exists");
                return Ok($"File '{request.ObjectName1}' isn't exists");
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //---------------------------------------------------自主访问控制---------------------------------------------------//
            //  判断用户是否有权限来写这个文件，只有 2 和 4 能写文件
            if (!HasPermission(user.ID, request.ObjectName1, 2) && !HasPermission(user.ID, request.ObjectName1, 4))
            {
                TypeLog(request.UserName, "coverFile", true, $"FileName:{request.ObjectName1}", false,
                    "User does not have Cover permission");
                return BadRequest("You do not have permission to Cover this file");
            }
            //---------------------------------------------------自主访问控制---------------------------------------------------//
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            //---------------------------------------------------强制访问控制---------------------------------------------------//
            //  目前还没开始写
            //---------------------------------------------------强制访问控制---------------------------------------------------//
            // 获取文件路径
            string filePath = Path.Combine(DirectoryPath, request.ObjectName1);

            // 写入文件
            System.IO.File.WriteAllText(filePath, request.Text+Environment.NewLine);

            TypeLog(request.UserName, "CoverFile", true, $"FileName:{request.ObjectName1}", true,
                "File written successfully");

            return Ok("File written successfully");

        }
        catch (Exception ex)
        {
            // 记录日志或返回错误信息
            return StatusCode(500, $"Error adding file: {ex.Message}");
        }
    }

    
   
}