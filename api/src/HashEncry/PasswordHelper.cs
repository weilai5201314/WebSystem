﻿using System.Security.Cryptography;
using System.Text;

namespace server.HashEncry
{
    public class PasswordHelper
    {
        // 定义盐值的字节数
        private const int SaltSize = 32;

        // 定义哈希密码的字节数
        private const int HashSize = 32;

        // 生成盐值和哈希密码的方法
        // public static (byte[] salt, byte[] hash) GenerateSaltAndHash(string password)
        // {
        //     // 使用 RNGCryptoServiceProvider 生成随机的盐值
        //     using (var rng = new RNGCryptoServiceProvider())
        //     {
        //         var salt = new byte[SaltSize];
        //         rng.GetBytes(salt);
        //
        //         // 调用 HashPassword 方法生成哈希密码
        //         var hash = HashPassword(password, salt);
        //
        //         // 返回生成的盐值和哈希密码
        //         return (salt, hash);
        //     }
        // }

        // 生成哈希密码的方法
        public static byte[] HashPassword(string password, byte[] salt)
        {
            // 使用 Rfc2898DeriveBytes 类进行密码哈希化，迭代次数为 10000
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 1000))
            {
                return pbkdf2.GetBytes(HashSize);
            }
        }

        // // 验证密码的方法
        // public static bool VerifyPassword(string password, byte[] salt, byte[] storedHash)
        // {
        //     // 重新生成哈希密码并与存储的哈希密码比较，以验证密码是否匹配
        //     var newHash = HashPassword(password, salt);
        //     return newHash.SequenceEqual(storedHash);
        // }

        // 验证 n 次密码的方法
        public static bool ComparePasswords(string inputPassword, byte[] storedHash, byte[] r, int n)
        {
            // 使用 VerifyPassword2 进行密码比较，对用户输入的密码进行 n 次迭代
            var hashedInputPassword = HashForLogin2(inputPassword, r, n);

            // 比较哈希值是否相同
            return hashedInputPassword.SequenceEqual(storedHash);
        }

        

        //  n 次迭代
        public static byte[] HashForLogin2(string password, byte[] r, int n)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, r, n))
            {
                
                return pbkdf2.GetBytes(HashSize);
            }
        }

        // 生成随机的迭代次数 N
        public static int GetRandomN()
        {
            // 这里可以根据需求设置合适的范围，比如 1 到 1000 之间
            //  在测试得时候可以调小范围，便于调试
            var random = new Random();
            return random.Next(3, 5);
        }

        // 生成随机数 R
        public static byte[] GenerateRandomR()
        {
            // 这里设置随机数的字节数，可以根据需求调整
            const int saltSize = 32;

            using (var rng = new RNGCryptoServiceProvider())
            {
                var salt = new byte[saltSize];
                rng.GetBytes(salt);
                return salt;
            }
        }
    }
}