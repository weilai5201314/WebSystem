﻿using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using server.HashEncry;
using server.Mysql.Data;
using server.Mysql.Models;
using server.Time;

namespace server.Controllers
{
    public partial class Api
    {
        // 构造注入数据库结构
        [FromServices] public MysqlDbContext DbContext { get; set; }


        [HttpPost("user/LogIn")]
        public IActionResult LogIn([FromBody] LogInRequest request)
        {
            var user = DbContext.User.FirstOrDefault(u => u.Account == request.Account);

            if (user == null)
            {
                // 用户不存在，记录登录失败的日志
                LogLogin(0, request.Account, false, "Invalid username ");
                return BadRequest("Invalid username ");
            }

            // 判断账号状态，只有 2 正常
            // if (user.Status == 2) 
            // 判断密码是否正确
            if (PasswordHelper.VerifyPassword(request.Password, user.Salt, user.Pass))
            {
                
                // 判断状态能否登录
                if (user.Status == 2)
                {
                    // 密码验证通过，创建 JWT Token
                    var token = GenerateJwtToken(user);

                    // 记录登录成功的日志
                    LogLogin(user.ID, request.Account, true, token);

                    // 返回成功登录的响应，包括 Token
                    return Ok(new { token });
                }
                
                // 记录登录失败的日志
                LogLogin(user.ID, request.Account, false, "请等待管理审核。");
                return BadRequest("请等待管理审核。");
            }

            LogLogin(user.ID, request.Account, false, "Invalid  password");
            return BadRequest("Invalid  password");
           
        }

        // 添加记录登录操作的日志函数
        private void LogLogin(int userId, string userName, bool success, string returnvalue)
        {
            DateTime cstTime = TimeHelper.BeijingTime;
            // 创建日志实体
            var log = new Log
            {
                Timestamp = cstTime,
                User = userName,
                Action = "Login",
                InputResult = success,
                InputValue = $"UserID: {userId}, UserName: {userName}, Success: {success}",
                ReturnResult = success,
                ReturnValue = returnvalue // 这里可以记录生成的 Token 或错误消息
            };

            // 将日志实体添加到数据库中
            DbContext.Log.Add(log);
            DbContext.SaveChanges();
        }


        // 获取jwt得token序列
        private string GenerateJwtToken(server.Mysql.Models.User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration["JwtSettings:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.ID.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30), // 设置 Token 过期时间
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    // 用户登录请求模型
    public class LogInRequest
    {
        [Required(ErrorMessage = "Account is required.")]
        [RegularExpression("^[a-zA-Z0-9]{1,20}$", ErrorMessage = "账号只能是20位以内的数字或者字母.")]
        public string Account { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression("^[a-zA-Z0-9!@#$%^&*()_+\\-=[\\]{};:'\\\",<.>/?]+$",
            ErrorMessage =
                "Password must be alphanumeric and may include special characters. It should be less than or equal to 30 characters.")]
        public string Password { get; set; }
    }
}