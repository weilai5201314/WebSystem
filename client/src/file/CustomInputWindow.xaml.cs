using System.Windows;

namespace client.file;

public partial class CustomInputWindow : Window
{
    public string UserInput { get; private set; }
    public CustomInputWindow(string currentContent)
    {
        InitializeComponent();
        CurrentContentTextBlock.Text = currentContent;
    }
    
    private void ConfirmButton_Click(object sender, RoutedEventArgs e)
    {
        UserInput = InputTextBox.Text;
        DialogResult = true;
    }
}