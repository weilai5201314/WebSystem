using System.Windows;
using client.user;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace client.admin;

public partial class ShowInfo : Window
{
    public ShowInfo()
    {
        InitializeComponent();
        LoadUserData();
    }

    private string i = LogIn.UserInfoAll.UserAccount;

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
}