using BookWorld.Services;
using CommunityToolkit.Mvvm.Input;

namespace BookWorld.ViewModels;

public partial class CashierViewModel
{
    private readonly ISaleService _sales;
    private readonly IChequeService _cheques;

    [RelayCommand]
    private async Task ConfirmSaleAsync()
    {
        ErrorMessage = null;
        try
        {
            var sale = await _sales.ConfirmAsync(Cart);
            if (!sale.IsSuccess)
            {
                ErrorMessage = sale.Error;
                await FindAsync();
                return;
            }

            var cheque = await _cheques.GetAsync(sale.Value);
            if (cheque is null)
            {
                ErrorMessage = $"Продажа № {sale.Value} сохранена, но чек сформировать не удалось.";
            }
            else
            {
                _windows.ShowCheque(new ChequeViewModel(cheque));
                StatusMessage = $"Продажа № {sale.Value} завершена.";
            }

            Cart.Clear();
            await FindAsync();
        }
        catch (Exception ex)
        {
            Report(ex);
        }
    }
}
