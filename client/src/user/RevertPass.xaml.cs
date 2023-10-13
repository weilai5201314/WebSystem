using System.Windows;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace client.user;

public partial class RevertPass : Window
{
    public RevertPass()
    {
        InitializeComponent();
    }

    private void ToRevertPass(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Account.Text) || string.IsNullOrWhiteSpace(Password.Text) || string.IsNullOrWhiteSpace(Password2.Text))
        {
            MessageBox.Show("请填写所有必要信息。");
            return;
        }

        if (Password.Text != Password2.Text)
        {
            MessageBox.Show("请输入相同的密码。");
            return;
        }

        bool result = ChangePassword(Account.Text, Password.Text);
        // 修改成功去新页面
        if (result)
        {
            // LogIn newWindow = new LogIn();
            RevertPass.GetWindow(this)?.Close();
            // newWindow.Show();
        }
    }

    private bool ChangePassword(string usaccount, string uspassword)
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
            var response = client.PostAsync("http://localhost:5009/Api/user/RevertPass", content).Result;

            // 判断网络状态码
            if (response.IsSuccessStatusCode)
            {
                // 修改密码成功
                var successMessage = response.Content.ReadAsStringAsync().Result;
                MessageBox.Show(successMessage, "修改密码成功");
                return true;
            }
            else
            {
                // 修改密码失败
                var errorMessage = response.Content.ReadAsStringAsync().Result;
                MessageBox.Show(errorMessage, "修改密码失败");
                return false;
            }
        }
    }

}