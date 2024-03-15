// using Microsoft.EntityFrameworkCore;
// using server.Mysql.Data;
//
// var builder = WebApplication.CreateBuilder(args);
//
// // Add services to the container.
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
//
// // 数据库添加配置
// builder.Configuration.AddJsonFile("appsettings.json", optional: false);
// // 添加数据库上下文
// builder.Services.AddDbContext<MysqlDbContext>(options =>
// {
//     options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty);
// });
//
//
// var app = builder.Build();
//
// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
//
// app.UseHttpsRedirection();
//
// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };
//
// app.MapGet("/weatherforecast", () =>
//     {
//         var forecast = Enumerable.Range(1, 5).Select(index =>
//                 new WeatherForecast
//                 (
//                     DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//                     Random.Shared.Next(-20, 55),
//                     summaries[Random.Shared.Next(summaries.Length)]
//                 ))
//             .ToArray();
//         return forecast;
//     })
//     .WithName("GetWeatherForecast")
//     .WithOpenApi();
//
// app.Run();
//
// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }

using Microsoft.EntityFrameworkCore;
using server.Mysql.Data;

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
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty);
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
    // 如果是 GET 请求，则不进行验证
    if (context.Request.Method == "GET")
    {
        await next.Invoke();
        return;
    }

    var path = context.Request.Path.Value;
    // 测试专用判断条件
    if (path != "/Api")
    {
        await next.Invoke();
        return;
    }

    // 如果没有提供有效的 Authorization 头或验证失败，返回未授权状态码
    context.Response.StatusCode = 411;
});

app.Run();