using Microsoft.AspNetCore.Mvc;

namespace server.Controllers.File
{
    [ApiController]
    [Route("[controller]/File")]
    public partial class Api : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello,File!");
        }

        [HttpPost("DeleteFile")]
        public IActionResult DeleteFile([FromBody] FileAccessRequest request)
        {
            // 处理删除文件的逻辑
            return Ok($"Deleting file: {request.ObjectName1}");
        }

       

        [HttpPost("WriteFile")]
        public IActionResult WriteFile([FromBody] FileAccessRequest request)
        {
            // 处理写文件的逻辑
            return Ok($"Writing file: {request.ObjectName1}");
        }

        [HttpPost("ReadWriteFile")]
        public IActionResult ReadWriteFile([FromBody] FileAccessRequest request)
        {
            // 处理读写文件的逻辑
            return Ok($"Reading and writing file: {request.ObjectName1}");
        }

        [HttpPost("CopyFile")]
        public IActionResult CopyFile([FromBody] FileAccessRequest request)
        {
            // 处理复制文件的逻辑
            return Ok($"Copying file from: {request.ObjectName1} to {request.ObjectName2}");
        }
    }

    public class FileAccessRequest
    {
        public string UserName { get; set; }
        public string ObjectName1 { get; set; }
        public string ObjectName2 { get; set; }
        public string Action { get; set; }
        public string Text { get; set; }
    }
}