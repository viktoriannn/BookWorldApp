using BookWorldApp.Services;
using BookWorldApp.ViewModels;

namespace BookWorldApp.Infrastructure;

public interface IWindowService
{
    void ShowLogin();
    void OpenWorkspace(CurrentUser user);
    void ShowCheque(ChequeViewModel viewModel);
}
