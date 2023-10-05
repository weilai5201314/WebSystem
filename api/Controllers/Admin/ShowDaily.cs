// using server.Mysql.Models;
// using System.Linq; // 导入 LINQ 查询命名空间
// using Microsoft.Extensions.Configuration;
// using System;
// using Microsoft.AspNetCore.Mvc;
// using server.HashEncry;
// using server.Mysql.Data;

// 导入数据注解命名空间

using Microsoft.AspNetCore.Mvc;

namespace server.Controllers.Admin
{
    public partial class Api
    {
        [HttpPost("ShowDaily")]
        public IActionResult ShowDaily()
        {
            return Ok("Hello,ShowDaily.");
        }
    }
}