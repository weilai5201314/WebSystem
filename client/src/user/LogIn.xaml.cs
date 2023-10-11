using System;
using System.Windows.Controls;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace client.user;

public partial class LogIn : Window
{
    public LogIn()
    {
        InitializeComponent();
    }


    private void ToLogIn(object sender, RoutedEventArgs e)
    {
        bool result = LogToGetToken(Account.Text, Password.Text);
        if (result)
        {
            // 登录成功，去新页面，存储token
            Jump_Mainwindos();
        }
        else
        {
            Password.Text = "";
        }
    }

    /// 跳转到主页面函数
    /// <param name="window"></param>
    private void Jump_Mainwindos()
    {
        MainWindow mainWindow = new MainWindow();
        LogIn.GetWindow(this)?.Close();
        mainWindow.Show();
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

            if (response.IsSuccessStatusCode)
            {
                // 登录成功
                var responseContent = response.Content.ReadAsStringAsync().Result;

                try
                {
                    var jsonResponse = JsonConvert.DeserializeObject<JObject>(responseContent);
                    if (jsonResponse.ContainsKey("token"))
                    {
                        TokenManager.LogInToken = jsonResponse["token"].Value<string>();
                        return true; // 登录成功
                    }
                    else
                    {
                        // 服务器返回的 JSON 中没有 token，表示登录失败
                        MessageBox.Show("登录失败");
                        return false;
                    }
                }
                catch (JsonReaderException)
                {
                    // 无法解析 JSON，表示登录失败
                    // var errorResponse = response.Content.ReadAsStringAsync().Result;
                    // MessageBox.Show(errorResponse, "登录失败");
                    MessageBox.Show( "登录失败");
                    return false;
                }
            }
            else
            {
                // 处理请求失败的情况
                var errorResponse = response.Content.ReadAsStringAsync().Result;
                MessageBox.Show(errorResponse, "登录失败");
                // MessageBox.Show( "登录失败");
                return false;
            }
        }
    }

    // 全局静态token
    public static class TokenManager
    {
        public static string LogInToken { get; set; }
    }
}