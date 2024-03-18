using System;
using System.IO;
using System.Windows;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
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
            InitializeFileComboBox(this);
        }

        //  初始化下拉框
        private async void InitializeFileComboBox(Window currentWindow)
        {
            try
            {
                // 构建请求数据
                var requestData = new
                {
                    userName = LogIn.UserInfoAll.UserAccount,
                    objectName1 = "string.txt",
                    objectName2 = "string.txt",
                    action = 1,
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
                    currentWindow.Close(); // 关闭窗口
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing file list: {ex.Message}");
                currentWindow.Close(); // 关闭窗口
            }
        }

        //  创建文件按钮
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
                    Action = 0,
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

        // 删除文件
        // 删除文件
        private async void Button_DeleteFile(object sender, RoutedEventArgs e)
        {
            try
            {
                // 获取下拉框中的文件名
                string selectedFileName = FileComboBox.SelectedItem as string;
                if (string.IsNullOrEmpty(selectedFileName))
                {
                    MessageBox.Show("Please select a file to delete.");
                    return;
                }

                // 构建请求数据
                var requestData = new
                {
                    userName = LogIn.UserInfoAll.UserAccount,
                    objectName1 = selectedFileName,
                    objectName2 = "string.txt",
                    action = 0,
                    text = "string"
                };

                // 发起请求，处理返回结果
                var response = PostRequest("http://localhost:5009/Api/File/DeleteFile", requestData);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(result);
                }
                else
                {
                    string errorResult = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(errorResult);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting file: {ex.Message}");
            }
        }

        // 读文件
        private async void Button_ReadFile(object sender, RoutedEventArgs e)
        {
            try
            {
                // 获取下拉框中的文件名
                string selectedFileName = FileComboBox.SelectedItem as string;
                if (string.IsNullOrEmpty(selectedFileName))
                {
                    MessageBox.Show("Please select a file to delete.");
                    return;
                }

                // 构建请求数据
                var requestData = new
                {
                    userName = LogIn.UserInfoAll.UserAccount,
                    objectName1 = selectedFileName,
                    objectName2 = "string.txt",
                    action = 0,
                    text = "string"
                };

                // 发起请求，处理返回结果
                var response = PostRequest("http://localhost:5009/Api/File/ReadFile", requestData);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(result, "读取内容");
                }
                else
                {
                    string errorResult = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(errorResult);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading file: {ex.Message}");
            }
        }

        //  写文件
        // 写入文件
        private async void Button_WriteFile(object sender, RoutedEventArgs e)
        {
            try
            {
                // 获取选中的文件名
                string selectedFileName = FileComboBox.SelectedItem as string;

                if (string.IsNullOrEmpty(selectedFileName))
                {
                    MessageBox.Show("Please select a file to write.");
                    return;
                }

                // 弹出窗口让用户输入内容
                string userInput =
                    Microsoft.VisualBasic.Interaction.InputBox("Enter text to write:", "Write to File", "");

                // 构建请求数据
                var requestData = new
                {
                    userName = LogIn.UserInfoAll.UserAccount,
                    objectName1 = selectedFileName,
                    objectName2 = "string.txt",
                    action = 0,
                    text = userInput
                };

                // 发起请求，处理返回结果
                var response = PostRequest("http://localhost:5009/Api/File/WriteFile", requestData);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("File written successfully");
                }
                else
                {
                    string errorResult = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(errorResult);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error writing to file: {ex.Message}");
            }
        }

        // 读写文件
        private async void Button_ReadAndWrite(object sender, RoutedEventArgs e)
        {
            try
            {
                // 获取选中的文件名
                string? selectedFileName = FileComboBox.SelectedItem as string;

                if (string.IsNullOrEmpty(selectedFileName))
                {
                    MessageBox.Show("请选择要读取和写入的文件。");
                    return;
                }

                // 发起读取文件的请求
                var readResponse = await GetFileContent(selectedFileName);

                if (readResponse != null)
                {
                    // 读取文件内容
                    string? currentContent = readResponse;
                    // 使用 InputBox 显示当前内容并允许用户输入新文本
                    string userInput =
                        Microsoft.VisualBasic.Interaction.InputBox("当前内容：\n\n" + currentContent + "\n\n请输入新文本:", "写入文件",
                            currentContent);
                    // 发起写文件的请求
                    var writeResponse = WriteFile(selectedFileName, userInput);
                    if (writeResponse.Result)
                        MessageBox.Show("文件读取并成功写入。");
                    else
                        MessageBox.Show($"写入文件时出错");
                }
                else
                    MessageBox.Show($"从文件读取时出错");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"读取和写入文件时出错：{ex.Message}");
            }
        }

        private async Task<string?> GetFileContent(string? selectedFileName)
        {
            // 构建请求数据
            var requestData = new
            {
                userName = LogIn.UserInfoAll.UserAccount,
                objectName1 = selectedFileName,
                objectName2 = "string.txt",
                action = 0,
                text = "string"
            };
            // 发起请求，处理返回结果
            var response = PostRequest("http://localhost:5009/Api/File/ReadFile", requestData);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();
            return null;
        }

        private Task<bool> WriteFile(string fileName, string text)
        {
            try
            {
                // 构建请求数据
                var requestData = new
                {
                    userName = LogIn.UserInfoAll.UserAccount,
                    objectName1 = fileName,
                    objectName2 = "string.txt",
                    action = 0,
                    Text = text
                };
                // 发起请求，处理返回结果
                var response = PostRequest("http://localhost:5009/Api/File/CoverFile", requestData);
                if (response.IsSuccessStatusCode)
                    return Task.FromResult(true);
                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                return Task.FromResult(false);
            }
        }

        // 附加token请求头，发起请求，传入 url 和 request请求 集合就行
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