using System.Collections.ObjectModel;
using BookWorldApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BookWorldApp.ViewModels;

public partial class CashierViewModel
{
    private readonly IAvailabilityService _availability;

    public ObservableCollection<ShopAvailability> Availability { get; } = new();

    [ObservableProperty] private string _availabilityMessage = "";

    partial void OnSelectedBookChanged(BookListItem? value) => _ = LoadAvailabilityAsync(value);

    private async Task LoadAvailabilityAsync(BookListItem? book)
    {
        Availability.Clear();
        AvailabilityMessage = "";
        if (book is null) return;

        try
        {
            var report = await _availability.GetAsync(book.BookId);
            if (SelectedBook?.BookId != book.BookId) return;

            Fill(Availability, report.Shops);
            AvailabilityMessage = report.Message;
        }
        catch (Exception ex)
        {
            Report(ex);
        }
    }
}
