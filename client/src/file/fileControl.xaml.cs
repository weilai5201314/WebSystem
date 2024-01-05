using System;
using System.IO;
using System.Windows;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using client.user;
using Microsoft.Win32;

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
                    objectName1 = "string.txt",
                    objectName2 = "string.txt",
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

        private void Button_CreateFile(object sender, RoutedEventArgs e)
        {
            // 选择txt文件
            var openFileDialog = new OpenFileDialog
            {
                Title = "选择文件",
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName; // 获取文件绝对路径
                string fileName = Path.GetFileName(filePath); // 获取文件名
                string fileContent = File.ReadAllText(filePath); // 获取文件文本内容
                // 构建请求数据
                var requestData = new
                {
                    UserName = LogIn.UserInfoAll.UserAccount,
                    ObjectName1 = fileName,
                    ObjectName2 = "string.txt",
                    Action = "string",
                    Text = fileContent 
                };

                // 发起请求，处理返回结果
                var response = PostRequest("http://localhost:5009/Api/File/AddFile", requestData);
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    MessageBox.Show(result);
                }
                else
                {
                    MessageBox.Show(response.Content.ReadAsStringAsync().Result);
                }
            }
        }


        private HttpResponseMessage PostRequest(string url, object requestData)
        {
            using (var client = new HttpClient())
            {
                // 添加请求头，附加 token
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LogIn.UserInfoAll.LogInToken);
                var requestDataJson = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(requestDataJson, Encoding.UTF8, "application/json");
                // 发送请求
                return client.PostAsync(url, content).Result;
            }
        }
    }
}