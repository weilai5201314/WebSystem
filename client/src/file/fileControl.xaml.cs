using System.Windows;

namespace client.file;

public partial class fileControl : Window
{
    public fileControl()
    {
        InitializeComponent();
    }
    
    private async void ReadFile_Click(object sender, RoutedEventArgs e)
    {
        // 调用后端API，读取文件
        // 更新界面或展示文件详情
    }

    private async void DeleteFile_Click(object sender, RoutedEventArgs e)
    {
        // 调用后端API，删除文件
        // 更新界面或显示删除成功信息
    }

}