using System;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text.Json;
using client.config.time;

namespace client.user;

public partial class LogIn : Window
{
    private const string ConfigFilePath = "D:\\zzz\\school\\InfoSecurity\\WebSystem\\client\\src\\config\\LogIn.json"; // JSON 配置文件路径

    public LogIn()
    {
        InitializeComponent();
    }

    private LoginConfig ReadConfig()
    {
        if (File.Exists(ConfigFilePath))
        {
            string json = File.ReadAllText(ConfigFilePath);
            return System.Text.Json.JsonSerializer.Deserialize<LoginConfig>(json); // 使用 System.Text.Json
        }

        return new LoginConfig();
    }

    // 写入 JSON 配置文件
    private void WriteConfig(LoginConfig config)
    {
        string json = System.Text.Json.JsonSerializer.Serialize(config); // 使用 System.Text.Json
        File.WriteAllText(ConfigFilePath, json);
    }

    // 登录函数，带登录失败次数限制和锁定时间
    private void ToLogIn(object sender, RoutedEventArgs e)
    {
        var config = ReadConfig();
        DateTime cstTime = TimeHelper.BeijingTime;
        if (config.LockoutEndUtc.HasValue && config.LockoutEndUtc > cstTime)
        {
            // 用户被锁定，不能登录
            MessageBox.Show($"登录已被锁定，请稍后再试。", "登录失败");
            return;
        }

        bool result = LogToGetToken(Account.Text, Password.Password);
        if (result)
        {
            // 登录成功，重置登录失败次数和锁定时间
            config.FailedLoginAttempts = 0;
            config.LockoutEndUtc = null;
            WriteConfig(config);

            // 登录成功，去新页面，存储 token
            Jump_Mainwindos();
        }
        else
        {
            // 登录失败，增加登录失败次数
            config.FailedLoginAttempts++;

            if (config.FailedLoginAttempts >= config.MaxLoginAttempts)
            {
                // 达到最大登录尝试次数，锁定用户
                DateTime cstTime2 = TimeHelper.BeijingTime;
                config.LockoutEndUtc = cstTime2.AddMinutes(config.LockoutDurationMinutes);
            }

            WriteConfig(config);
            Password.Password = "";
        }
    }

    // private void ToLogIn(object sender, RoutedEventArgs e)
    // {
    //     bool result = LogToGetToken(Account.Text, Password.Password);
    //     if (result)
    //     {
    //         // 登录成功，去新页面，存储token
    //         Jump_Mainwindos();
    //     }
    //     else
    //     {
    //         Password.Password = "";
    //     }
    // }

    /// 跳转到主页面函数
    /// <param name="window"></param>
    private void Jump_Mainwindos()
    {
        MainWindow mainWindow = new MainWindow();
        LogIn.GetWindow(this)?.Close();
        mainWindow.Show();
    }

    /// 跳转到注册页面函数
    /// <param name="window"></param>
    private void Jump_SignUp(object sender, RoutedEventArgs e)
    {
        SignUp newWindow = new SignUp();
        this.IsEnabled = false; //禁用原来的窗口
        // 订阅新窗口的 Closed 事件，在窗口关闭时恢复原始窗口的可用状态
        newWindow.Closed += (sender, e) =>
        {
            this.IsEnabled = true;
            //  当修改页面关闭后，才 清空输入
            //     Account.Text = "";
            //     Tip.Text = "";
        };
        newWindow.Show();
    }

    // 获取token，向服务器发起请求，接收返回
    // 输入：账号，密码
    // 返回：  成功，返回：token
    //        失败，返回：不同的string字符串
    private bool LogToGetToken(string usaccount, string uspassword)
    {
        using (var client = new HttpClient())
        {
            // 构建请求数据
            var requestData = new
            {
                account = usaccount,
                password = uspassword
            };

            // 将请求数据转换为 JSON
            var requestDataJson = JsonConvert.SerializeObject(requestData);

            // 创建 HTTP 请求消息
            var content = new StringContent(requestDataJson, Encoding.UTF8, "application/json");

            // 发起 POST 请求
            var response = client.PostAsync("http://localhost:5009/Api/user/LogIn", content).Result;

            // 判断网络状态码
            if (response.IsSuccessStatusCode)
            {
                // 登录成功
                var responseContent = response.Content.ReadAsStringAsync().Result;

                try
                {
                    // 解析json相应
                    var jsonResponse = JsonConvert.DeserializeObject<JObject>(responseContent);
                    if (jsonResponse.ContainsKey("token"))
                    {
                        // 赋值token给全局变量 LogInToken
                        UserInfoAll.UserAccount = usaccount;
                        UserInfoAll.LogInToken = jsonResponse["token"].Value<string>();
                        return true; // 登录成功
                    }
                    else
                    {
                        // 服务器返回的 JSON 中没有 token，表示登录失败
                        var errorResponse = response.Content.ReadAsStringAsync().Result;
                        MessageBox.Show(errorResponse, "登录失败");
                        return false;
                    }
                }
                catch (JsonReaderException)
                {
                    // 无法解析 JSON，表示登录失败
                    var errorResponse = response.Content.ReadAsStringAsync().Result;
                    MessageBox.Show(errorResponse, "登录失败");
                    // MessageBox.Show("登录失败");
                    return false;
                }
            }
            else
            {
                // 处理请求失败的情况
                var errorResponse = response.Content.ReadAsStringAsync().Result;
                MessageBox.Show(errorResponse, "登录失败");
                // MessageBox.Show("登录失败");
                return false;
            }
        }
    }

    // 全局用户信息
    public static class UserInfoAll
    {
        public static string UserAccount { get; set; }
        public static string LogInToken { get; set; }
    }

    // 隐藏密码功能
    private void ShowPasswordCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        // 显示密码
        PasswordText.Text = Password.Password;
        Password.Visibility = Visibility.Collapsed;
        PasswordText.Visibility = Visibility.Visible;
    }

    private void ShowPasswordCheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        // 隐藏密码
        PasswordText.Visibility = Visibility.Collapsed;
        Password.Visibility = Visibility.Visible;
    }

    private void Password_PasswordChanged(object sender, RoutedEventArgs e)
    {
        // 当 PasswordBox 中的密码更改时，同步更新 TextBox 中的密码
        PasswordText.Text = Password.Password;
    }

    public class LoginConfig
    {
        public int MaxLoginAttempts { get; set; } = 3;
        public int LockoutDurationMinutes { get; set; } = 5;
        public int FailedLoginAttempts { get; set; } = 0;
        public DateTime? LockoutEndUtc { get; set; }
    }
}