using System.Windows;
using BookWorldApp.ViewModels;

namespace BookWorldApp.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is not LoginViewModel viewModel) return;

        viewModel.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(LoginViewModel.Password) && viewModel.Password.Length == 0)
                PasswordInput.Clear();
        };
    }

    private void PasswordInput_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is LoginViewModel viewModel)
            viewModel.Password = PasswordInput.Password;
    }
}
