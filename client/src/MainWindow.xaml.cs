using System.Windows;
using client.admin;
using client.file;
// using client.File;
using client.user;


namespace client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // 前往用户信息页面
        private void ToShowInfo(object sender, RoutedEventArgs e)
        {
            ShowInfo newWindow = new ShowInfo();
            IsEnabled = false; //禁用原来的窗口
            // 订阅新窗口的 Closed 事件，在窗口关闭时恢复原始窗口的可用状态
            newWindow.Closed += (sender, e) =>
            {
                IsEnabled = true;
                //  当修改页面关闭后，才 清空输入
                //     Account.Text = "";
                //     Tip.Text = "";
            };
            newWindow.Show();
        }

        // 前往日志页面
        private void ToShowDaily(object sender, RoutedEventArgs e)
        {
            ShowDaily newWindow = new ShowDaily();
            IsEnabled = false; //禁用原来的窗口
            // 订阅新窗口的 Closed 事件，在窗口关闭时恢复原始窗口的可用状态
            newWindow.Closed += (sender, e) =>
            {
                this.IsEnabled = true;
                //  当修改页面关闭后，才 清空输入
                //     Account.Text = "";
                //     Tip.Text = "";
            };
            newWindow.Show();
        }
        
        //  前往找回密码页面
        private void ToRevertPass(object sender, RoutedEventArgs e)
        {
            RevertPass newWindow = new RevertPass();
            IsEnabled = false; //禁用原来的窗口
            // 订阅新窗口的 Closed 事件，在窗口关闭时恢复原始窗口的可用状态
            newWindow.Closed += (sender, e) =>
            {
                this.IsEnabled = true;
                //  当修改页面关闭后，才 清空输入
                //     Account.Text = "";
                //     Tip.Text = "";
            };
            newWindow.Show();
        }

        private void Button_LogOut(object sender, RoutedEventArgs e)
        {
            
            LogIn newWindow = new LogIn();
            newWindow.Show();
            GetWindow(this)?.Close();
        }

        private void JumpFileControl(object sender, RoutedEventArgs e)
        {
            fileControl newWindow = new fileControl();
            IsEnabled = false;
            newWindow.Closed += (sender, e) =>
            {
                IsEnabled = true;
            };
            newWindow.Show();
        }
    }
}