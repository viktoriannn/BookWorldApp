using System.Diagnostics;
using BookWorld.Infrastructure;
using BookWorld.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookWorld.ViewModels;

public partial class CashierViewModel : ObservableObject
{
    private const string DatabaseError = "Не удалось выполнить операцию: нет связи с базой данных.";

    private readonly UserSession _session;
    private readonly IWindowService _windows;

    public CashierViewModel(
        UserSession session,
        IWindowService windows,
        ICatalogService catalog,
        IReferenceService references,
        IAvailabilityService availability)
    {
        _session = session;
        _windows = windows;
        _catalog = catalog;
        _references = references;
        _availability = availability;
    }

    public string ShopAddress => _session.User?.ShopName ?? "";

    public string UserInfo => $"Пользователь: {_session.User?.Login} · Роль: {Roles.Display(_session.User?.Role ?? "")}";

    [ObservableProperty] private string? _errorMessage;
    [ObservableProperty] private string _statusMessage = "";

    [RelayCommand]
    private void Logout()
    {
        _session.SignOut();
        _windows.ShowLogin();
    }

    private void Report(Exception ex)
    {
        Debug.WriteLine(ex);
        ErrorMessage = ex is UnauthorizedAccessException ? ex.Message : DatabaseError;
    }
}
