using System.ComponentModel.DataAnnotations;
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
        [Required(ErrorMessage = "Username is required.")]
        [RegularExpression("^[a-zA-Z0-9]{1,20}$",
            ErrorMessage = "Username can only contain up to 20 alphanumeric characters.")]
        public string UserName { get; set; }

        [RegularExpression("^[\\w\\p{IsCJKUnifiedIdeographs}]{1,30}\\.txt$",
            ErrorMessage =
                "ObjectName1 should be a filename with up to 30 alphanumeric characters ending with '.txt'.")]
        public string ObjectName1 { get; set; }

        [RegularExpression("^[\\w\\p{IsCJKUnifiedIdeographs}]{1,30}\\.txt$",
            ErrorMessage =
                "ObjectName2 should be a filename with up to 30 alphanumeric characters ending with '.txt'.")]
        public string ObjectName2 { get; set; }

        public string Action { get; set; }

        [MaxLength(100, ErrorMessage = "Text can have a maximum length of 100 characters.")]
        public string Text { get; set; }
    }

    public class FilePermissionRequest
    {
        [Required(ErrorMessage = "Username is required.")]
        [RegularExpression("^[a-zA-Z0-9]{1,20}$",
            ErrorMessage = "Username can only contain up to 20 alphanumeric characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [RegularExpression("^[a-zA-Z0-9]{1,20}$",
            ErrorMessage = "Username can only contain up to 20 alphanumeric characters.")]
        public string AlterUserName { get; set; }

        [RegularExpression("^[\\w\\p{IsCJKUnifiedIdeographs}]{1,30}\\.txt$",
            ErrorMessage =
                "ObjectName1 should be a filename with up to 30 alphanumeric characters ending with '.txt'.")]
        public string FileName { get; set; }
        
        public int Action { get; set; }

        public int Permission { get; set; }
    }
}