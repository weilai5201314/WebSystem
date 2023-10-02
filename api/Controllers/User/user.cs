using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace server.Controllers.User
{
    public partial class Api : ControllerBase
    {
        private readonly Dictionary<string, (byte[] salt, byte[] hash)> users = new Dictionary<string, (byte[] salt, byte[] hash)>();

        [HttpPost("user/SignUp")]
        public IActionResult SignUp([FromBody] SignUpRequest request)
        {
            if (users.ContainsKey(request.Username))
            {
                return BadRequest("Username already exists.");
            }

            byte[] salt = PasswordHelper.GenerateSalt();
            byte[] hash = PasswordHelper.HashPassword(request.Password, salt);

            users[request.Username] = (salt, hash);

            return Ok("User registered successfully");
        }

        [HttpPost("user/LogIn")]
        public IActionResult LogIn([FromBody] LogInRequest request)
        {
            if (users.TryGetValue(request.Username, out var storedCredentials))
            {
                if (PasswordHelper.VerifyPassword(request.Password, storedCredentials.salt, storedCredentials.hash))
                {
                    return Ok("Login successful");
                }
            }

            return BadRequest("Invalid username or password");
        }
    }

    public class SignUpRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LogInRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class PasswordHelper
    {
        private const int SaltSize = 32;
        private const int HashSize = 32;

        public static byte[] GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var salt = new byte[SaltSize];
                rng.GetBytes(salt);
                return salt;
            }
        }

        public static byte[] HashPassword(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                return pbkdf2.GetBytes(HashSize);
            }
        }

        public static bool VerifyPassword(string password, byte[] salt, byte[] hash)
        {
            var newHash = HashPassword(password, salt);
            return newHash.SequenceEqual(hash);
        }
    }
}
