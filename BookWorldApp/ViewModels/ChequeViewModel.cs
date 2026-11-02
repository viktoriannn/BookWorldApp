using System.Globalization;
using BookWorldApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookWorldApp.ViewModels;

public partial class ChequeViewModel(ChequeInfo cheque) : ObservableObject
{
    private const string DateFormat = "dd.MM.yyyy HH:mm";

    public ChequeInfo Cheque { get; } = cheque;

    public string Info
        => $"{Cheque.ShopName} | Кассир: {Cheque.CashierLogin} | {Cheque.Date.ToString(DateFormat, CultureInfo.CurrentCulture)}";

    public string Message => "Продажа сохранена, остаток уменьшен.";

    public event Action<bool>? CloseRequested;

    [RelayCommand]
    private void Issued() => CloseRequested?.Invoke(true);

    [RelayCommand]
    private void Back() => CloseRequested?.Invoke(false);
}
