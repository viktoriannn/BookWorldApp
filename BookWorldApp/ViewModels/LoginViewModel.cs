using System.Diagnostics;
using BookWorldApp.Infrastructure;
using BookWorldApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookWorldApp.ViewModels;

public partial class LoginViewModel(IAuthService auth, IWindowService windows) : ObservableObject
{
    private const string ConnectionError = "Не удалось подключиться к базе данных. Обратитесь к администратору.";

    [ObservableProperty] private string _login = "";
    [ObservableProperty] private string _password = "";
    [ObservableProperty] private string? _errorMessage;

    [RelayCommand]
    private async Task LoginAsync()
    {
        ErrorMessage = null;
        try
        {
            var result = await auth.LoginAsync(Login, Password);
            if (!result.IsSuccess)
            {
                ErrorMessage = result.Error;
                Password = "";
                return;
            }

            windows.OpenWorkspace(result.Value!);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            ErrorMessage = ConnectionError;
        }
    }
}
