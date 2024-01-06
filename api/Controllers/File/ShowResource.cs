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
            var fileInfo = DbContext.Resource.Select(u => u.FileName).ToList();

            bool successful = fileInfo.Count > 0;
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