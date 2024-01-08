using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows;
using client.user;
using Newtonsoft.Json;

namespace client.admin;

public partial class AlterFileAuth : Window
{
    public AlterFileAuth()
    {
        InitializeComponent();

        InitializePermissionBox(this);
        InitializeFileBox();
        InitializeUserBox();
    }

    private async void Button_AddWritePermission(object sender, RoutedEventArgs e)
    {
        try
        {
            // 获取所选文件和用户
            var selectedFile = FileComboBox.SelectedItem as string;
            var selectedUser = UserComboBox.SelectedItem as string;

            if (string.IsNullOrEmpty(selectedFile) || string.IsNullOrEmpty(selectedUser))
            {
                MessageBox.Show("Please select a file and a user to grant read permission.");
                return;
            }

            // 构建请求数据
            var requestData = new
            {
                userName = LogIn.UserInfoAll.UserAccount,
                alterUserName = selectedUser,
                fileName = selectedFile,
                action = 1, // 1 表示增加权限
                permission = 2 // 1 表示读权限
            };

            // 将请求数据转为 JSON 字符串
            var requestDataJson = JsonConvert.SerializeObject(requestData);

            // 构建 HTTP 请求内容
            var content = new StringContent(requestDataJson, Encoding.UTF8, "application/json");

            // 构建 HTTP 客户端请求
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LogIn.UserInfoAll.LogInToken);

            // 发起 POST 请求更新文件权限
            var response = await httpClient.PostAsync("http://localhost:5009/Api/File/UpdateFilePermission", content);

            // 检查请求是否成功
            if (response.IsSuccessStatusCode)
            {
                // 读取响应内容
                var responseContent = await response.Content.ReadAsStringAsync();

                // 处理后端返回的结果，例如显示成功消息
                MessageBox.Show(responseContent, "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // 刷新权限列表
                InitializePermissionBox(this);
            }
            else
            {
                MessageBox.Show("Error updating file permission.");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating file permission: {ex.Message}");
        }
    }

    private async void Button_AddReadPermission(object sender, RoutedEventArgs e)
    {
        try
        {
            // 获取所选文件和用户
            var selectedFile = FileComboBox.SelectedItem as string;
            var selectedUser = UserComboBox.SelectedItem as string;

            if (string.IsNullOrEmpty(selectedFile) || string.IsNullOrEmpty(selectedUser))
            {
                MessageBox.Show("Please select a file and a user to grant read permission.");
                return;
            }

            // 构建请求数据
            var requestData = new
            {
                userName = LogIn.UserInfoAll.UserAccount,
                alterUserName = selectedUser,
                fileName = selectedFile,
                action = 1, // 1 表示增加读权限
                permission = 1 // 1 表示读权限
            };

            // 将请求数据转为 JSON 字符串
            var requestDataJson = JsonConvert.SerializeObject(requestData);

            // 构建 HTTP 请求内容
            var content = new StringContent(requestDataJson, Encoding.UTF8, "application/json");

            // 构建 HTTP 客户端请求
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LogIn.UserInfoAll.LogInToken);

            // 发起 POST 请求更新文件权限
            var response = await httpClient.PostAsync("http://localhost:5009/Api/File/UpdateFilePermission", content);

            // 检查请求是否成功
            if (response.IsSuccessStatusCode)
            {
                // 读取响应内容
                var responseContent = await response.Content.ReadAsStringAsync();

                // 处理后端返回的结果，例如显示成功消息
                MessageBox.Show(responseContent, "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // 刷新权限列表
                InitializePermissionBox(this);
            }
            else
            {
                MessageBox.Show("Error updating file permission.");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating file permission: {ex.Message}");
        }
    }

    private async void Button_DeletePermission(object sender, RoutedEventArgs e)
    {
        try
        {
            // 获取所选行的数据
            var selectedPermission = PermissionDataGrid.SelectedItem as dynamic;
            if (selectedPermission == null)
            {
                MessageBox.Show("Please select a permission to delete.");
                return;
            }

            // 构建请求数据
            var requestData = new
            {
                userName = LogIn.UserInfoAll.UserAccount,
                alterUserName = selectedPermission.User,
                fileName = selectedPermission.ilename,
                action = 2, // 2 表示删除权限
                permission = selectedPermission.ermission
            };

            // 将请求数据转为 JSON 字符串
            var requestDataJson = JsonConvert.SerializeObject(requestData);

            // 构建 HTTP 请求内容
            var content = new StringContent(requestDataJson, Encoding.UTF8, "application/json");

            // 构建 HTTP 客户端请求
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LogIn.UserInfoAll.LogInToken);

            // 发起 POST 请求更新文件权限
            var response = await httpClient.PostAsync("http://localhost:5009/Api/File/UpdateFilePermission", content);

            // 检查请求是否成功
            if (response.IsSuccessStatusCode)
            {
                // 读取响应内容
                var responseContent = await response.Content.ReadAsStringAsync();

                // 处理后端返回的结果，例如显示成功消息
                MessageBox.Show(responseContent, "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // 刷新权限列表
                InitializePermissionBox(this);
            }
            else
            {
                MessageBox.Show("Error updating file permission.");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating file permission: {ex.Message}");
        }
    }

    private void Button_FlushAll(object sender, RoutedEventArgs e)
    {
        InitializeFileBox();
        // InitializePermissionBox();
        InitializeComponent();
    }

    private async void InitializePermissionBox(Window currentWindow)
    {
        try
        {
            // 构建请求数据
            var requestData = new
            {
                userName = LogIn.UserInfoAll.UserAccount,
                objectName1 = "string.txt",
                objectName2 = "string.txt",
                action = 3,
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

                // 将返回的 JSON 字符串解析成对象列表
                var permissionList = JsonConvert.DeserializeObject<List<PermissionResponse>>(responseContent);

                // 将对象列表添加到 DataGrid
                // PermissionDataGrid.ItemsSource = permissionList;
                foreach (var permission in permissionList)
                {
                    PermissionDataGrid.Items.Add(new
                    {
                        User = permission.Account,
                        ilename = permission.FileName,
                        ermission = permission.PermissionCode
                    });
                }
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

    private async void InitializeFileBox()
    {
        try
        {
            // 构建请求数据
            var requestData = new
            {
                userName = LogIn.UserInfoAll.UserAccount,
                objectName1 = "string.txt",
                objectName2 = "string.txt",
                action = 2,
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

    private async void InitializeUserBox()
    {
        try
        {
            // 构建请求数据
            var requestData = new
            {
                userName = LogIn.UserInfoAll.UserAccount,
                objectName1 = "string.txt",
                objectName2 = "string.txt",
                action = 4,
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
                    UserComboBox.Items.Add(fileName);
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

    public class PermissionResponse
    {
        public string Account { get; set; }
        public string FileName { get; set; }
        public int PermissionCode { get; set; }
    }
}