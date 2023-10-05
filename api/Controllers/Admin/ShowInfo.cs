using Microsoft.AspNetCore.Mvc;


// 导入数据注解命名空间

namespace server.Controllers.Admin;

public partial class Api
{

    [HttpPost("ShowInfo")]
    public IActionResult ShowInfo()
    {
        

        return Ok("Hello,ShowInfo.");
    }


}


