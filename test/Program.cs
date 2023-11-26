using System;
using System.Security.Cryptography;
using System.Text;

public class PasswordHashExample
{
    private const int SaltSize = 32;
    private const int HashSize = 32;

    public static byte[] HashPassword(string password, byte[] salt, int iterations)
    {
        using (var sha256 = new SHA256Managed())
        {
            byte[] hash = Encoding.UTF8.GetBytes(password + BitConverter.ToString(salt).Replace("-", ""));

            for (int i = 0; i < iterations; i++)
            {
                hash = sha256.ComputeHash(hash);
            }

            return hash;
        }
    }

    public static void Main()
    {
        // 假设这是服务器端
        string password = "your_password";
        byte[] salt = GenerateRandomSalt();

        int n = 3;

        // 一次性迭代 n+1 次
        byte[] hashOnce = HashPassword(password, salt, n + 1);

        // 分别迭代 n 次和 1 次
        byte[] hashIterateN = HashPassword(password, salt, n);
        byte[] hashIterate1 = HashPassword(Encoding.UTF8.GetString(hashIterateN), salt, 1);

        Console.WriteLine("一次性迭代 n+1 次结果: " + BitConverter.ToString(hashOnce).Replace("-", ""));
        Console.WriteLine("先迭代 n 次再迭代 1 次结果: " + BitConverter.ToString(hashIterate1).Replace("-", ""));

        Console.WriteLine("两者结果是否相同: " + BitConverter.ToString(hashOnce).Replace("-", "").Equals(BitConverter.ToString(hashIterate1).Replace("-", "")));
    }

    private static byte[] GenerateRandomSalt()
    {
        using (var rng = new RNGCryptoServiceProvider())
        {
            var salt = new byte[SaltSize];
            rng.GetBytes(salt);
            return salt;
        }
    }
}