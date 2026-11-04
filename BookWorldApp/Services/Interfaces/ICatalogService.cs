namespace BookWorldApp.Services;

public interface ICatalogService
{
    Task<IReadOnlyList<BookListItem>> SearchAsync( SearchCriteria criteria, CancellationToken ct = default);
}