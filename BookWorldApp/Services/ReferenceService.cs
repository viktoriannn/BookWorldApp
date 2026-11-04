using BookWorldApp.Data;
using Microsoft.EntityFrameworkCore;

namespace BookWorldApp.Services;

public record LookupItem(int Id, string Name);

public record ReferenceData(
    IReadOnlyList<LookupItem> Genres,
    IReadOnlyList<LookupItem> Authors,
    IReadOnlyList<LookupItem> Publishers,
    IReadOnlyList<LookupItem> Translators,
    IReadOnlyList<LookupItem> Series);

public class ReferenceService : IReferenceService
{
    private readonly IDbContextFactory<BookStoreContext> factory;
    private readonly UserSession session;

    public ReferenceService(IDbContextFactory<BookStoreContext> factory, UserSession session)
    {
        this.factory = factory;
        this.session = session;
    }

    public async Task<ReferenceData> GetAsync(CancellationToken ct = default)
    {
        session.RequireCashier();
        await using var db = await factory.CreateDbContextAsync(ct);

        var genres = await db.Genres.AsNoTracking().OrderBy(x => x.Title)
            .Select(x => new LookupItem(x.IdGenre, x.Title)).ToListAsync(ct);

        var publishers = await db.PublishingHouses.AsNoTracking().OrderBy(x => x.Title)
            .Select(x => new LookupItem(x.IdPublishingHouse, x.Title)).ToListAsync(ct);

        var series = await db.SeriesSet.AsNoTracking().OrderBy(x => x.Title)
            .Select(x => new LookupItem(x.IdSeries, x.Title)).ToListAsync(ct);

        var authors = await db.Authors.AsNoTracking()
            .OrderBy(x => x.Surname).ThenBy(x => x.Name)
            .Select(x => new { x.IdAuthor, x.Surname, x.Name, x.Patronymic })
            .ToListAsync(ct);

        var translators = await db.Translators.AsNoTracking()
            .OrderBy(x => x.Surname).ThenBy(x => x.Name)
            .Select(x => new { x.IdTranslator, x.Surname, x.Name, x.Patronymic })
            .ToListAsync(ct);

        return new ReferenceData(
            genres,
            authors.Select(x => new LookupItem(x.IdAuthor,
                PersonName.Full(x.Surname, x.Name, x.Patronymic))).ToList(),
            publishers,
            translators.Select(x => new LookupItem(x.IdTranslator,
                PersonName.Full(x.Surname, x.Name, x.Patronymic))).ToList(),
            series);
    }
}