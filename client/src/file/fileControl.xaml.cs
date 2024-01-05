using System;
using System.Windows;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using client.user;

namespace client.file
{
    public partial class fileControl : Window
    {
        public fileControl()
        {
            InitializeComponent();

            // 初始化 ComboBox
            InitializeFileComboBox();
        }

        private async void InitializeFileComboBox()
        {
            try
            {
                // 构建请求数据
                var requestData = new
                {
                    userName = LogIn.UserInfoAll.UserAccount,
                    objectName1 = "string",
                    objectName2 = "string",
                    action = "string",
                    text = "string"
                };
                // 将请求数据转为 JSON 字符串
                var requestDataJson = JsonConvert.SerializeObject(requestData);
                // 构建 HTTP 请求内容
                var content = new StringContent(requestDataJson, Encoding.UTF8, "application/json");
                // 构建 HTTP 客户端请求
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LogIn.UserInfoAll.LogInToken);
                // 发起 POST 请求获取文件列表
                var response = await httpClient.PostAsync("http://localhost:5009/Api/File/ShowFile", content);
                // 检查请求是否成功
                if (response.IsSuccessStatusCode)
                {
                    // 读取响应内容
                    var responseContent = await response.Content.ReadAsStringAsync();
                    // 将返回的 JSON 字符串解析成文件列表
                    var fileList = JsonConvert.DeserializeObject<string[]>(responseContent);
                    // 将文件列表添加到 ComboBox
                    foreach (var fileName in fileList)
                        FileComboBox.Items.Add(fileName);
                }
                else
                {
                    MessageBox.Show("Error initializing file list.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing file list: {ex.Message}");
            }
        }
    }
}