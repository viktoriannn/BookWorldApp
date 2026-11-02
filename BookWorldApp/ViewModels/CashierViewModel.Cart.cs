using BookWorld.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookWorld.ViewModels;

public partial class CashierViewModel
{
    public BookCart Cart { get; } = new();

    [ObservableProperty] private CartLine? _selectedCartLine;
    [ObservableProperty] private int _quantityText = 1;

    [RelayCommand]
    private void AddToCart()
    {
        ErrorMessage = null;
        if (SelectedBook is null)
        {
            ErrorMessage = "Выберите книгу в списке.";
            return;
        }

        if (SelectedBook.Stock <= 0)
        {
            ErrorMessage = "Этой книги нет в вашем магазине. Проверьте наличие в других магазинах сети.";
            return;
        }

        var result = Cart.Add(SelectedBook.BookId, SelectedBook.Title, SelectedBook.Price, QuantityText, SelectedBook.Stock);
        if (!result.IsSuccess) ErrorMessage = result.Error;
    }

    [RelayCommand]
    private void RemoveLine()
    {
        ErrorMessage = null;
        if (SelectedCartLine is null)
        {
            ErrorMessage = "Выберите позицию чека.";
            return;
        }

        Cart.Remove(SelectedCartLine.BookId);
    }

    [RelayCommand]
    private void ClearCart()
    {
        ErrorMessage = null;
        Cart.Clear();
    }
}
