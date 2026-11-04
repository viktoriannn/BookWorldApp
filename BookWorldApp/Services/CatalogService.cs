using BookWorldApp.Data;
using BookWorldApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BookWorldApp.Services;
public record BookListItem(
    int BookId,
    string Title,
    string Authors,
    string Genres,
    string Publisher,
    string Isbn,
    decimal Price,
    int Stock);

public class CatalogService : ICatalogService
{
    private const int MaxResults = 500;
    private const string LikeEscape = "\\";

    private readonly IDbContextFactory<BookStoreContext> factory;
    private readonly UserSession session;

    public CatalogService(IDbContextFactory<BookStoreContext> factory, UserSession session)
    {
        this.factory = factory;
        this.session = session;
    }

    public async Task<IReadOnlyList<BookListItem>> SearchAsync(
        SearchCriteria criteria, CancellationToken ct = default)
    {
        var shopId = session.RequireCashier().ShopId!.Value;

        await using var db = await factory.CreateDbContextAsync(ct);

        var query = ApplyFilters(
            ApplyText(db.Books.AsNoTracking(), criteria), criteria);

        var rows = await query
            .OrderBy(b => b.Title)
            .Take(MaxResults)
            .Select(b => new
            {
                b.IdBook,
                b.Title,
                b.Isbn,
                b.Price,
                Publisher = b.PublishingHouse.Title,
                Authors = b.BookAuthors.Select(x =>
                    new { x.Author.Surname, x.Author.Name, x.Author.Patronymic }).ToList(),
                Genres = b.BookGenres.Select(x => x.Genre.Title).ToList(),
                Stock = b.Remains
                    .Where(r => r.IdShop == shopId)
                    .Select(r => r.NumberOfCopies)
                    .FirstOrDefault()
            })
            .ToListAsync(ct);

        return rows.Select(r => new BookListItem(
            r.IdBook,
            r.Title,
            string.Join(", ", r.Authors.Select(a =>
                PersonName.Short(a.Surname, a.Name, a.Patronymic))),
            string.Join(", ", r.Genres),
            r.Publisher,
            r.Isbn,
            r.Price,
            r.Stock)).ToList();
    }

    public static string LikePattern(string text) =>
        "%" + text.Replace("\\", "\\\\")
                  .Replace("%", "\\%")
                  .Replace("_", "\\_")
                  .Replace("[", "\\[") + "%";

    private static IQueryable<Book> ApplyText(
        IQueryable<Book> query, SearchCriteria criteria)
    {
        var text = criteria.Text?.Trim();

        if (string.IsNullOrEmpty(text))
            return query;

        var p = LikePattern(text);

        return criteria.Kind switch
        {
            SearchKind.Title =>
                query.Where(b => EF.Functions.Like(b.Title, p, LikeEscape)),

            SearchKind.Author =>
                query.Where(b => b.BookAuthors.Any(x => EF.Functions.Like(
                    x.Author.Surname + " " + x.Author.Name + " " +
                    (x.Author.Patronymic ?? ""), p, LikeEscape))),

            SearchKind.Genre =>
                query.Where(b => b.BookGenres.Any(x =>
                    EF.Functions.Like(x.Genre.Title, p, LikeEscape))),

            SearchKind.Publisher =>
                query.Where(b => EF.Functions.Like(
                    b.PublishingHouse.Title, p, LikeEscape)),

            SearchKind.Translator =>
                query.Where(b => b.BookTranslators.Any(x => EF.Functions.Like(
                    x.Translator.Surname + " " + x.Translator.Name + " " +
                    (x.Translator.Patronymic ?? ""), p, LikeEscape))),

            SearchKind.Series =>
                query.Where(b => b.Series != null &&
                    EF.Functions.Like(b.Series.Title, p, LikeEscape)),

            _ => query.Where(b =>
                EF.Functions.Like(b.Title, p, LikeEscape) ||
                b.BookAuthors.Any(x => EF.Functions.Like(
                    x.Author.Surname + " " + x.Author.Name + " " +
                    (x.Author.Patronymic ?? ""), p, LikeEscape)) ||
                b.BookGenres.Any(x =>
                    EF.Functions.Like(x.Genre.Title, p, LikeEscape)) ||
                EF.Functions.Like(b.PublishingHouse.Title, p, LikeEscape) ||
                b.BookTranslators.Any(x => EF.Functions.Like(
                    x.Translator.Surname + " " + x.Translator.Name + " " +
                    (x.Translator.Patronymic ?? ""), p, LikeEscape)) ||
                (b.Series != null &&
                    EF.Functions.Like(b.Series.Title, p, LikeEscape)))
        };
    }

    private static IQueryable<Book> ApplyFilters(
        IQueryable<Book> query, SearchCriteria criteria)
    {
        if (criteria.GenreId is int id)
            query = query.Where(b => b.BookGenres.Any(x => x.IdGenre == id));

        if (criteria.AuthorId is int id2)
            query = query.Where(b => b.BookAuthors.Any(x => x.IdAuthor == id2));

        if (criteria.PublisherId is int id3)
            query = query.Where(b => b.IdPublishingHouse == id3);

        if (criteria.TranslatorId is int id4)
            query = query.Where(b => b.BookTranslators.Any(x => x.IdTranslator == id4));

        if (criteria.SeriesId is int id5)
            query = query.Where(b => b.IdSeries == id5);

        return query;
    }
}