using System;
using System.Windows;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using client.config.HashEncry;

namespace client.user;

public partial class SignUp : Window
{
    public SignUp()
    {
        InitializeComponent();
    }

    // 注册按钮
    private void Button_ToSignUp(object sender, RoutedEventArgs e)
    {
        if (Password.Text != Password2.Text)
        {
            MessageBox.Show("请输入相同的密码。");
            return;
        }

        int n = 0; //迭代次数
        byte[] r = new byte[32]; //随机数
        byte[] PassHash; // 用于存放加密后的密码
        byte[] PassHash1; // 用于存放加密后的密码
        // 先向第一个接口发起请求

        if (GetSignUp1(Account.Text, out n, out r))
        {
            // 开始加密,获取加密后返回值
            // 注意是注册阶段，迭代次数是 N+1 次
            PassHash1 = PassHelper.HashForLogin2(Password.Text, n, r);
            PassHash = PassHelper.HashForLogin2(Convert.ToBase64String(PassHash1), 1, r);
            // 开始向第二个接口发起请求
            if (GetSignUp2(Account.Text, PassHash))
            {
                // 成功就返回主菜单
                GetWindow(this)?.Close();
            }
        }
    }

    // 进行第一个接口的请求
    private bool GetSignUp1(string usaccount, out int n, out byte[] r)
    {
        n = 0;
        r = new byte[32];
        using (var client = new HttpClient())
        {
            // 构建请求数据
            var requestData = new
            {
                account = usaccount
            };
            // 将请求数据转换为 JSON
            var requestDataJson = JsonConvert.SerializeObject(requestData);
            // 创建 HTTP 请求消息
            var content = new StringContent(requestDataJson, Encoding.UTF8, "application/json");
            // 发起 POST 请求
            var response = client.PostAsync("http://localhost:5009/Api/user/SignUp", content).Result;
            // 判断网络状态码
            if (response.IsSuccessStatusCode)
            {
                // 注册成功，等待审核
                var successful = response.Content.ReadAsStringAsync().Result;
                // 解析返回的 JSON 数据
                var responseData = JsonConvert.DeserializeObject<FirstResponse>(successful);
                // 提取 n 和 r
                n = responseData.N;
                r = Convert.FromBase64String(responseData.R);
                // 这里可以继续使用 n 和 r 进行后续操作
                MessageBox.Show(successful, "注册成功");
                return true;
            }
            else
            {
                // 注册失败
                var errorResponse = response.Content.ReadAsStringAsync().Result;
                MessageBox.Show(errorResponse, "注册失败");
                return false;
            }
        }
    }

    // 解析请求响应
    public class FirstResponse
    {
        public int N { get; set; }
        public string R { get; set; }
    }

    // 进行第二个接口的请求
    private bool GetSignUp2(string useraccount, byte[] HashPass)
    {
        using (var client = new HttpClient())
        {
            // 构建请求数据
            var requestData = new
            {
                account = useraccount,
                password = Convert.ToBase64String(HashPass) // 将加密后的密码转换为 Base64 字符串
            };
            // 将请求数据转换为 JSON
            var requestDataJson = JsonConvert.SerializeObject(requestData);
            // 创建 HTTP 请求消息
            var content = new StringContent(requestDataJson, Encoding.UTF8, "application/json");
            // 发起 POST 请求
            var response = client.PostAsync("http://localhost:5009/Api/user/SignUp2", content).Result;
            // 判断网络状态码
            if (response.IsSuccessStatusCode)
            {
                // 注册成功，等待审核
                var successful = response.Content.ReadAsStringAsync().Result;
                MessageBox.Show(successful, "注册成功");
                // 成功后返回 true
                return true;
            }
            else
            {
                // 注册失败
                var errorResponse = response.Content.ReadAsStringAsync().Result;
                MessageBox.Show(errorResponse, "注册失败");
                // 失败时返回 false
                return false;
            }
        }
    }
}