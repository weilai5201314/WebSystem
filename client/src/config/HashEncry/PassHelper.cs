using System.Security.Cryptography;

namespace client.config.HashEncry;

public class PassHelper
{
    // 定义盐值的字节数
    private const int SaltSize = 32;

    // 定义哈希密码的字节数
    private const int HashSize = 32;


    //  迭代函数
    //  password，n次迭代，随机数r
    public static byte[] HashForLogin2(string password, int n, byte[] r)
    {
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, r, n))
        {
            return pbkdf2.GetBytes(HashSize);
        }
    }
}