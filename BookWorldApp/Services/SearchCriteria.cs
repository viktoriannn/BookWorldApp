namespace BookWorldApp.Services;

public enum SearchKind
{
    Everywhere, Title, Author, Genre, Publisher, Translator, Series
}

public record SearchCriteria(
    string? Text,
    SearchKind Kind = SearchKind.Everywhere,
    int? GenreId = null,
    int? AuthorId = null,
    int? PublisherId = null,
    int? TranslatorId = null,
    int? SeriesId = null);