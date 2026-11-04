using BookWorldApp.Data;
using Microsoft.EntityFrameworkCore;

namespace BookWorldApp.Services;

public record ChequeLineInfo(string Title, int Quantity, decimal Price, decimal Sum);

public record ChequeInfo(
    int ChequeId,
    DateTime Date,
    string ShopName,
    string CashierLogin,
    IReadOnlyList<ChequeLineInfo> Lines,
    decimal Total);

public class ChequeService : IChequeService
{
    private readonly IDbContextFactory<BookStoreContext> factory;
    private readonly UserSession session;

    public ChequeService(IDbContextFactory<BookStoreContext> factory, UserSession session)
    {
        this.factory = factory;
        this.session = session;
    }

    public async Task<ChequeInfo?> GetAsync(int chequeId, CancellationToken ct = default)
    {
        var shopId = session.RequireCashier().ShopId!.Value;
        await using var db = await factory.CreateDbContextAsync(ct);

        var row = await db.Cheques.AsNoTracking()
            .Where(c => c.IdCheque == chequeId && c.IdShop == shopId)
            .Select(c => new
            {
                c.IdCheque,
                c.Date,
                ShopName = c.Shop.Title,
                Cashier = c.Employee.Login,
                c.TotalSum,
                Lines = c.ChequeBooks
                    .OrderBy(x => x.IdChequeBook)
                    .Select(x => new { x.Book.Title, x.Count, x.Price })
                    .ToList()
            })
            .FirstOrDefaultAsync(ct);

        if (row is null) return null;

        var lines = row.Lines.Select(x =>
            new ChequeLineInfo(x.Title, x.Count, x.Price, x.Price * x.Count)).ToList();

        return new ChequeInfo(
            row.IdCheque, row.Date, row.ShopName, row.Cashier, lines, row.TotalSum);
    }
}