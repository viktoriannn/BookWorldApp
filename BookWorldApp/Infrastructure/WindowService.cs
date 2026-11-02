using System.Windows;
using BookWorldApp.Services;
using BookWorldApp.ViewModels;
using BookWorldApp.Views;
using Microsoft.Extensions.DependencyInjection;

namespace BookWorldApp.Infrastructure;

public sealed class WindowService(IServiceProvider services) : IWindowService
{
    private Window? _current;

    public void ShowLogin()
        => Replace(new LoginWindow { DataContext = services.GetRequiredService<LoginViewModel>() });

    public void OpenWorkspace(CurrentUser user)
    {
        switch (user.Role)
        {
            case Roles.Cashier:
                var viewModel = services.GetRequiredService<CashierViewModel>();
                Replace(new CashierWindow { DataContext = viewModel });
                break;
            case Roles.Admin:
                services.GetRequiredService<UserSession>().SignOut();
                MessageBox.Show("Рабочее место администратора не входит в текущую версию приложения.",
                    "BookWorld", MessageBoxButton.OK, MessageBoxImage.Information);
                break;
            default:
                throw new InvalidOperationException($"Неизвестная роль: {user.Role}");
        }
    }

    private void Replace(Window next)
    {
        var previous = _current;
        _current = next;
        Application.Current.MainWindow = next;
        next.Show();
        previous?.Close();
    }
}
