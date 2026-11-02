using System.Collections.ObjectModel;
using BookWorldApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookWorldApp.ViewModels;

public partial class CashierViewModel
{
    private readonly IReferenceService _references;

    public ObservableCollection<LookupItem> Genres { get; } = new();
    public ObservableCollection<LookupItem> Authors { get; } = new();
    public ObservableCollection<LookupItem> Publishers { get; } = new();
    public ObservableCollection<LookupItem> Translators { get; } = new();
    public ObservableCollection<LookupItem> SeriesList { get; } = new();

    [ObservableProperty] private LookupItem? _selectedGenre;
    [ObservableProperty] private LookupItem? _selectedAuthor;
    [ObservableProperty] private LookupItem? _selectedPublisher;
    [ObservableProperty] private LookupItem? _selectedTranslator;
    [ObservableProperty] private LookupItem? _selectedSeries;

    private async Task LoadFiltersAsync()
    {
        try
        {
            var data = await _references.GetAsync();
            Fill(Genres, data.Genres);
            Fill(Authors, data.Authors);
            Fill(Publishers, data.Publishers);
            Fill(Translators, data.Translators);
            Fill(SeriesList, data.Series);
        }
        catch (Exception ex)
        {
            Report(ex);
        }
    }

    [RelayCommand]
    private Task ApplyFiltersAsync() => FindAsync();

    [RelayCommand]
    private async Task ResetFiltersAsync()
    {
        SelectedGenre = null;
        SelectedAuthor = null;
        SelectedPublisher = null;
        SelectedTranslator = null;
        SelectedSeries = null;
        await FindAsync();
    }
}
