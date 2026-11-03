using BookWorldApp.Data;
using Microsoft.EntityFrameworkCore;

namespace BookWorldApp.Services;

public sealed record ShopAvailability(int ShopId, string ShopName, int Quantity, bool IsMyShop);

public sealed record AvailabilityReport(IReadOnlyList<ShopAvailability> Shops, string Message);

public interface IAvailabilityService
{
    Task<AvailabilityReport> GetAsync(int bookId, CancellationToken ct = default);
}

public sealed class AvailabilityService(IDbContextFactory<BookStoreContext> factory, UserSession session) : IAvailabilityService
{
    public async Task<AvailabilityReport> GetAsync(int bookId, CancellationToken ct = default)
    {
        var myShopId = session.RequireCashier().ShopId!.Value;
        await using var db = await factory.CreateDbContextAsync(ct);

        var rows = await db.Shops.AsNoTracking()
            .Select(s => new
            {
                s.IdShop,
                s.Title,
                Quantity = s.Remains.Where(r => r.IdBook == bookId).Select(r => r.NumberOfCopies).FirstOrDefault()
            })
            .ToListAsync(ct);

        var shops = rows
            .Select(r => new ShopAvailability(r.IdShop, r.Title, r.Quantity, r.IdShop == myShopId))
            .OrderByDescending(s => s.IsMyShop)
            .ThenBy(s => s.ShopName)
            .ToList();

        return new AvailabilityReport(shops, BuildMessage(shops));
    }

    public static string BuildMessage(IReadOnlyList<ShopAvailability> shops)
    {
        var mine = shops.Where(s => s.IsMyShop).Sum(s => s.Quantity);
        if (mine > 0) return $"В вашем магазине в наличии: {mine} экз. Книгу можно продать.";

        var others = shops.Where(s => !s.IsMyShop && s.Quantity > 0).ToList();
        return others.Count > 0
            ? "В вашем магазине книги нет. Есть в: " + string.Join("; ", others.Select(s => $"{s.ShopName} — {s.Quantity} экз."))
            : "Книги нет во всей сети.";
    }
}
