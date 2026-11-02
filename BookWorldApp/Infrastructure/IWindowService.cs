using BookWorld.Services;
using BookWorld.ViewModels;

namespace BookWorld.Infrastructure;

public interface IWindowService
{
    void ShowLogin();
    void OpenWorkspace(CurrentUser user);
    void ShowCheque(ChequeViewModel viewModel);
    bool? ShowOrderCreate(int? preselectedBookId);
}
