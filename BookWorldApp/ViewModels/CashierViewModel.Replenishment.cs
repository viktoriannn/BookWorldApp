using CommunityToolkit.Mvvm.Input;

namespace BookWorldApp.ViewModels;

public partial class CashierViewModel
{
    [RelayCommand]
    private void OpenReplenishment()
    {
        ErrorMessage = null;
        if (_windows.ShowOrderCreate(SelectedBook?.BookId) == true)
            StatusMessage = "Заказ на пополнение создан.";
    }
}
