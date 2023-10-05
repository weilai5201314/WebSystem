using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using server.Mysql.Data;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Primitives;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// 数据库添加配置
builder.Configuration.AddJsonFile("appsettings.json", optional: false);
// 添加数据库上下文
builder.Services.AddDbContext<MysqlDbContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// jwt验证的入口
app.UseAuthorization();

app.MapControllers();

// JWT验证方法
app.Use(async (context, next) =>
{
    // / 检查请求路径，如果是登录请求则不进行验证
    //  所有 Post 请求都加密，除了特定页面
    if ( context.Request.Method != "POST" ||
         context.Request.Path == "/Api/user/SignUp" ||  // 注册
         context.Request.Path == "/Api/user/LogIn" ||   // 登录
         context.Request.Path == "/Api/user/RevertPass"     // 找回密码
         )
    {
        await next.Invoke();
        return;
    }

    // 获取请求头中的 Authorization
    if (context.Request.Headers.TryGetValue("Authorization", out StringValues authHeader))
    {
        var token = authHeader.FirstOrDefault()?.Split(" ").Last(); // 获取 JWT Token
        var jwtSettingsKey = builder.Configuration["JwtSettings:Key"];// 获取密钥
        if (!string.IsNullOrEmpty(token))
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                // 验证 JWT Token，需要提供相应的密钥和验证参数
                var validationParameters = new TokenValidationParameters
                {
                    
                    // 设置密钥
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettingsKey)),
                    ValidateIssuer = false, // 是否验证发行者
                    ValidateAudience = false, // 是否验证受众
                };

                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // 将用户信息添加到 HttpContext 中，以便后续控制器中访问
                context.Items["User"] = principal;

                // 继续请求处理
                await next.Invoke();
                return;
            }
            catch (Exception ex)
            {
                // 验证失败，可以根据需要处理
                context.Response.StatusCode = 401; // 未授权
                return;
            }
        }
    }

    // 如果没有提供有效的 Authorization 头或验证失败，返回未授权状态码
    context.Response.StatusCode = 401;
});

app.Run();
