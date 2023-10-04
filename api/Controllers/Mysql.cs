using Microsoft.AspNetCore.Mvc;
using server.Mysql.Data;

namespace server.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    public class Mysql : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public Mysql(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _context.User.ToList();
            return Ok(users);
        }

        // 其他操作...
    }
}