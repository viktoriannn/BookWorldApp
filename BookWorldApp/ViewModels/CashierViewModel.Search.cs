using System.Collections.ObjectModel;
using BookWorldApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookWorldApp.ViewModels;

public partial class CashierViewModel
{
    private const string NothingFoundMessage = "По вашему запросу ничего не найдено. Измените условия поиска или сбросьте фильтры.";

    private readonly ICatalogService _catalog;
    private bool _isInitializing;

    public ObservableCollection<BookListItem> Results { get; } = new();

    [ObservableProperty] private string _searchText = "";
    [ObservableProperty] private BookListItem? _selectedBook;

    [RelayCommand]
    private async Task InitializeAsync()
    {
        _isInitializing = true;

        try
        {
            await LoadFiltersAsync();
            await FindAsync();
        }
        finally
        {
            _isInitializing = false;
        }
    }

    [RelayCommand]
    private async Task FindAsync()
    {
        ErrorMessage = null;
        var hasFilters = SelectedGenre is not null
            || SelectedAuthor is not null
            || SelectedPublisher is not null
            || SelectedTranslator is not null
            || SelectedSeries is not null;

        if (!_isInitializing && string.IsNullOrWhiteSpace(SearchText) && !hasFilters)
        {
            StatusMessage = "";
            ErrorMessage = "Введите слово или фразу для поиска книги.";
            return;
        }
        try
        {
            var previousBookId = SelectedBook?.BookId;
            var items = await _catalog.SearchAsync(BuildCriteria());
            Fill(Results, items);
            SelectedBook = Results.FirstOrDefault(item => item.BookId == previousBookId);
            StatusMessage = items.Count == 0 ? NothingFoundMessage : $"Найдено изданий: {items.Count}";
        }
        catch (Exception ex)
        {
            Report(ex);
        }
    }

    private SearchCriteria BuildCriteria() => new(
        SearchText,
        SearchKind.Everywhere,
        SelectedGenre?.Id,
        SelectedAuthor?.Id,
        SelectedPublisher?.Id,
        SelectedTranslator?.Id,
        SelectedSeries?.Id);

    private static void Fill<T>(ObservableCollection<T> target, IEnumerable<T> source)
    {
        target.Clear();
        foreach (var item in source) target.Add(item);
    }
}
