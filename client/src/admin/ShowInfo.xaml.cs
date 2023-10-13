using System.Windows;
using client.user;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace client.admin;

public partial class ShowInfo : Window
{
    public ShowInfo()
    {
        InitializeComponent();
        LoadUserData();
    }

    private void UpdateAllStatus(object sender, RoutedEventArgs e)
    {
        var selectedUsers = new List<UserInfo>();

        // 获取选中的用户
        foreach (UserInfo user in userDataTableGrid.SelectedItems)
        {
            selectedUsers.Add(user);
        }

        if (selectedUsers.Count == 0)
        {
            MessageBox.Show("请选择要批量修改状态的用户。");
            return;
        }


        foreach (var user in selectedUsers)
        {
            // 批量修改状态的逻辑
            var result = UpdateUserStatus(user.account, 2); // 修改状态
            // MessageBox.Show(result);
        }

        // 重新加载用户信息
        LoadUserData();
    }

    private async Task<string> UpdateUserStatus(string userAccount, int newstatus)
    {
        // 构建请求的 URL
        string apiUrl = "http://localhost:5009/Api/admin/UpdateStatus";

        // 构建请求数据
        var requestData = new
        {
            account = LogIn.UserInfoAll.UserAccount,
            alterAccount = userAccount,
            newStatus = newstatus
        };

        // 创建 HttpClient
        using (HttpClient client = new HttpClient())
        {
            // 设置请求头，包括 token
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LogIn.UserInfoAll.LogInToken);

            // 使用 PutAsJsonAsync 发起 PUT 请求
            HttpResponseMessage response = await client.PutAsJsonAsync(apiUrl, requestData);

            // 检查响应是否成功
            if (response.IsSuccessStatusCode)
            {
                // 读取并返回响应字符串
                string responseContent = await response.Content.ReadAsStringAsync();
                // MessageBox.Show(responseContent);
                return responseContent;
            }
            else
            {
                // 处理请求失败的情况，例如抛出异常或返回错误消息
                // string responseContent = await response.Content.ReadAsStringAsync();
                // MessageBox.Show(responseContent);
                return "Error: " + response.StatusCode.ToString();
            }
        }
    }


    // 从数据库加载用户数据
    private async void LoadUserData()
    {
        using (var client = new HttpClient())
        {
            try
            {
                // 构建请求数据
                var requestData = new
                {
                    account = LogIn.UserInfoAll.UserAccount
                };


                // 将请求数据转换为 JSON
                var requestDataJson = JsonConvert.SerializeObject(requestData);

                // 创建 HTTP 请求消息
                var content = new StringContent(requestDataJson, System.Text.Encoding.UTF8, "application/json");

                // 附加token
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LogIn.UserInfoAll.LogInToken);

                // 发起 POST 请求
                var response = await client.PostAsync("http://localhost:5009/Api/admin/ShowInfo", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var userData = JsonConvert.DeserializeObject<List<UserInfo>>(responseContent);

                    // 填充数据到 DataGrid
                    userDataTableGrid.ItemsSource = userData;
                }
                else
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(responseContent, "无法加载用户数据。");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误: " + ex.Message);
            }
        }
    }

    // 定义用户信息类
    private class UserInfo
    {
        public string account { get; set; }
        public int status { get; set; }
    }

    private void FlushStatus(object sender, RoutedEventArgs e)
    {
        LoadUserData();
    }

    // 删除用户按钮
    private void DeleteUser(object sender, RoutedEventArgs e)
    {
        var selectedUser = userDataTableGrid.SelectedItem as UserInfo;

        if (selectedUser == null)
        {
            MessageBox.Show("请选择要删除的用户。");
            return;
        }

        if (MessageBox.Show("确定要删除选中的用户吗？", "确认删除", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            // 删除用户的逻辑
            DeleteSelectedUser(selectedUser.account);


        // 重新加载用户信息
        LoadUserData();
    }

    private async Task<string> DeleteSelectedUser(string userAccount)
    {
        // 构建请求的 URL
        string apiUrl = "http://localhost:5009/Api/admin/DeleteUser";

        // 构建请求数据
        var requestData = new
        {
            adminAccount = LogIn.UserInfoAll.UserAccount,
            userAccount = userAccount
        };

        // 创建 HttpClient
        using (HttpClient client = new HttpClient())
        {
            // 设置请求头，包括 token
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LogIn.UserInfoAll.LogInToken);

            // 将数据序列化为 JSON 格式
            var requestDataJson = JsonConvert.SerializeObject(requestData);

            // 使用 StringContent 指定 JSON 数据和 Content-Type
            var content = new StringContent(requestDataJson, System.Text.Encoding.UTF8, "application/json");

            // 使用 HttpMethod.Delete 发起 DELETE 请求
            HttpResponseMessage response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, apiUrl)
                { Content = content });

            // 检查响应是否成功
            if (response.IsSuccessStatusCode)
            {
                // 删除成功
                return "User deleted successfully.";
            }
            else
            {
                // 处理请求失败的情况，例如抛出异常或返回错误消息
                return "Error: " + response.StatusCode.ToString();
            }
        }
    }
}