using System.Windows;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace client.user;

public partial class SignUp : Window
{
    public SignUp()
    {
        InitializeComponent();
    }

    // 注册按钮
    private void ToSignUp(object sender, RoutedEventArgs e)
    {
        if (Password.Text != Password2.Text)
        {
            MessageBox.Show("请输入相同的密码。");
            return;
        }

        bool result = RegisterUser(Account.Text, Password.Text);
        // 注册成功去新页面
        if (result)
        {
            // LogIn newWindow = new LogIn();
            SignUp.GetWindow(this)?.Close();
            // newWindow.Show();
        }
    }

    // 开始注册
    private bool RegisterUser(string usaccount, string uspassword)
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
            var response = client.PostAsync("http://localhost:5009/Api/user/SignUp", content).Result;

            // 判断网络状态码
            if (response.IsSuccessStatusCode)
            {
                // 注册成功，等待审核
                var successful = response.Content.ReadAsStringAsync().Result;
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
}