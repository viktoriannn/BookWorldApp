using BookWorldApp.Data;
using Microsoft.EntityFrameworkCore;

namespace BookWorldApp.Services;

public record ShopAvailability(
    int ShopId,
    string ShopName,
    int Quantity,
    bool IsMyShop);

public record AvailabilityReport(
    IReadOnlyList<ShopAvailability> Shops,
    string Message);

public class AvailabilityService : IAvailabilityService
{
    private readonly IDbContextFactory<BookStoreContext> _factory;
    private readonly UserSession _session;

    public AvailabilityService(
        IDbContextFactory<BookStoreContext> factory,
        UserSession session)
    {
        _factory = factory;
        _session = session;
    }

    public async Task<AvailabilityReport> GetAsync(
        int bookId,
        CancellationToken ct = default)
    {
        int myShopId = _session.RequireCashier().ShopId!.Value;

        using var db = await _factory.CreateDbContextAsync(ct);

        var rows = await db.Shops
            .AsNoTracking()
            .Select(s => new
            {
                s.IdShop,
                s.Title,
                Quantity = s.Remains
                    .Where(r => r.IdBook == bookId)
                    .Select(r => r.NumberOfCopies)
                    .FirstOrDefault()
            })
            .ToListAsync(ct);

        var shops = rows
            .Select(s => new ShopAvailability(
                s.IdShop,
                s.Title,
                s.Quantity,
                s.IdShop == myShopId))
            .OrderByDescending(s => s.IsMyShop)
            .ThenBy(s => s.ShopName)
            .ToList();

        int myQuantity = shops
            .Where(s => s.IsMyShop)
            .Sum(s => s.Quantity);

        string message;

        if (myQuantity > 0)
        {
            message = $"В вашем магазине в наличии: {myQuantity} экз. Книгу можно продать.";
        }
        else
        {
            var otherShops = shops
                .Where(s => !s.IsMyShop && s.Quantity > 0)
                .ToList();

            if (otherShops.Count > 0)
            {
                message = "В вашем магазине книги нет. Есть в: " +
                    string.Join("; ", otherShops.Select(s =>
                        $"{s.ShopName} — {s.Quantity} экз."));
            }
            else
            {
                message = "Книги нет во всей сети.";
            }
        }

        return new AvailabilityReport(shops, message);
    }
}