using System;
using System.Windows;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using client.user;

namespace client.admin;

public partial class ShowDaily : Window
{
    public ShowDaily()
    {
        InitializeComponent();

        // 加载日志
        LoadLogData();
    }

    // 刷新一下日志
    private void ReLoadDaily(object sender, RoutedEventArgs e)
    {
        LoadLogData();
    }

    // 初始化日志函数
    private async void LoadLogData()
    {
        // 这里假设你已经获得了 Token 并存储在全局变量 LogIn.UserInfoAll.LogInToken 中

        // 构建请求数据
        var requestData = new
        {
            account = LogIn.UserInfoAll.UserAccount
        };

        //  发送网络请求
        using (var client = new HttpClient())
        {
            // 构建请求头，附加token
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LogIn.UserInfoAll.LogInToken);
            var requestDataJson = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(requestDataJson, System.Text.Encoding.UTF8, "application/json");
            // 请求端口
            var response = await client.PostAsync("http://localhost:5009/Api/admin/ShowDaily", content);
            // 判断相应
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var logEntries = JsonConvert.DeserializeObject<List<LogEntry>>(responseContent);

                // 将数据绑定到 DataGrid
                LogDataGrid.ItemsSource = logEntries;
            }
            else
            {
                MessageBox.Show("无法获取日志信息。");
            }
        }
    }

    public class LogEntry
    {
        public int ID { get; set; }

        public DateTime Timestamp { get; set; }

        public string User { get; set; }

        public string Action { get; set; }

        public bool InputResult { get; set; }

        public string InputValue { get; set; }

        public bool ReturnResult { get; set; }

        public string ReturnValue { get; set; }
    }
}