using System.Windows;
using BookWorld.Services;
using BookWorld.ViewModels;
using BookWorld.Views;
using Microsoft.Extensions.DependencyInjection;

namespace BookWorld.Infrastructure;

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
                viewModel.InitializeCommand.Execute(null);
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

    public void ShowCheque(ChequeViewModel viewModel)
    {
        var window = new ChequeWindow { DataContext = viewModel, Owner = _current };
        viewModel.CloseRequested += _ => CloseIfOpen(window);
        window.ShowDialog();
    }

    private void Replace(Window next)
    {
        var previous = _current;
        _current = next;
        Application.Current.MainWindow = next;
        next.Show();
        previous?.Close();
    }

    private static void CloseIfOpen(Window window)
    {
        if (window.IsVisible) window.Close();
    }
}
