using System.Windows;

namespace BookWorldApp.Views;

public partial class ConfirmWindow : Window
{
    private const string DefaultMessage = "Удалить выбранную запись? Это действие нельзя отменить.";

    public ConfirmWindow() : this(DefaultMessage)
    {
    }

    public ConfirmWindow(string message)
    {
        InitializeComponent();
        MessageText.Text = message;
    }

    private void Confirm_Click(object sender, RoutedEventArgs e) => DialogResult = true;
}
