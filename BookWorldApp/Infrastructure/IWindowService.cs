using BookWorldApp.Services;

namespace BookWorldApp.Infrastructure;

public interface IWindowService
{
    void ShowLogin();
    void OpenWorkspace(CurrentUser user);
}
