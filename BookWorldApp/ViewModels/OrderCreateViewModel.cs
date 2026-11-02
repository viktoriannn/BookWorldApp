using System.Collections.ObjectModel;
using System.Diagnostics;
using BookWorldApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookWorldApp.ViewModels;

public partial class OrderCreateViewModel : ObservableObject
{
    private const string DatabaseError = "Не удалось выполнить операцию: нет связи с базой данных.";

    private readonly UserSession _session;
    private readonly ICatalogService _catalog;
    private readonly IAvailabilityService _availability;
    private readonly IReplenishmentService _orders;

    public OrderCreateViewModel(
        UserSession session,
        ICatalogService catalog,
        IAvailabilityService availability,
        IReplenishmentService orders)
    {
        _session = session;
        _catalog = catalog;
        _availability = availability;
        _orders = orders;
    }

    public string ShopName => _session.User?.ShopName ?? "";

    public string OrderDate { get; } = DateTime.Now.ToShortDateString();

    public ObservableCollection<BookListItem> Books { get; } = new();
    public BookCart Cart { get; } = new();

    [ObservableProperty] private BookListItem? _selectedBook;
    [ObservableProperty] private int _quantity = 1;
    [ObservableProperty] private CartLine? _selectedLine;
    [ObservableProperty] private string _availabilityMessage = "";
    [ObservableProperty] private string? _errorMessage;

    public event Action<bool>? CloseRequested;

    public async Task InitializeAsync(int? preselectedBookId)
    {
        try
        {
            foreach (var book in await _catalog.SearchAsync(new SearchCriteria(null))) Books.Add(book);
            SelectedBook = Books.FirstOrDefault(book => book.BookId == preselectedBookId);
        }
        catch (Exception ex)
        {
            Report(ex);
        }
    }

    [RelayCommand]
    private void AddBook()
    {
        ErrorMessage = null;
        if (SelectedBook is null)
        {
            ErrorMessage = "Выберите книгу из списка.";
            return;
        }


        var result = Cart.Add(SelectedBook.BookId, SelectedBook.Title, SelectedBook.Price, Quantity, stockLimit: null);
        if (!result.IsSuccess) ErrorMessage = result.Error;
    }

    [RelayCommand]
    private void RemoveLine()
    {
        ErrorMessage = null;
        if (SelectedLine is null)
        {
            ErrorMessage = "Выберите позицию заказа.";
            return;
        }

        Cart.Remove(SelectedLine.BookId);
    }

    [RelayCommand]
    private async Task CheckAvailabilityAsync()
    {
        ErrorMessage = null;
        if (SelectedBook is null)
        {
            ErrorMessage = "Выберите книгу из списка.";
            return;
        }

        try
        {
            AvailabilityMessage = (await _availability.GetAsync(SelectedBook.BookId)).Message;
        }
        catch (Exception ex)
        {
            Report(ex);
        }
    }

    [RelayCommand]
    private async Task CreateAsync()
    {
        ErrorMessage = null;
        try
        {
            var result = await _orders.CreateAsync(Cart);
            if (!result.IsSuccess)
            {
                ErrorMessage = result.Error;
                return;
            }

            CloseRequested?.Invoke(true);
        }
        catch (Exception ex)
        {
            Report(ex);
        }
    }

    [RelayCommand]
    private void Cancel() => CloseRequested?.Invoke(false);

    private void Report(Exception ex)
    {
        Debug.WriteLine(ex);
        ErrorMessage = ex is UnauthorizedAccessException ? ex.Message : DatabaseError;
    }
}
