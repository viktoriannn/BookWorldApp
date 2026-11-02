using BookWorld.Data;
using BookWorld.Models;
using Microsoft.EntityFrameworkCore;

namespace BookWorld.Services;

public interface ISaleService
{
    Task<Result<int>> ConfirmAsync(BookCart cart, CancellationToken ct = default);
}

public sealed class SaleService(IDbContextFactory<BookStoreContext> factory, UserSession session) : ISaleService
{
    public async Task<Result<int>> ConfirmAsync(BookCart cart, CancellationToken ct = default)
    {
        var user = session.RequireCashier();
        var shopId = user.ShopId!.Value;

        if (cart.IsEmpty) return Result<int>.Fail("Чек пуст. Добавьте хотя бы одну книгу.");
        if (cart.FirstError() is { } error) return Result<int>.Fail(error);

        await using var db = await factory.CreateDbContextAsync(ct);
        await using var transaction = await db.Database.BeginTransactionAsync(ct);

        var bookIds = cart.Lines.Select(l => l.BookId).ToList();
        var books = await db.Books.Where(b => bookIds.Contains(b.IdBook)).ToDictionaryAsync(b => b.IdBook, ct);

        var cheque = new Cheque { Date = DateTime.Now, IdShop = shopId, IdEmployee = user.EmployeeId };

        foreach (var line in cart.Lines)
        {
            if (!books.TryGetValue(line.BookId, out var book))
                return Result<int>.Fail($"Книга «{line.Title}» больше не числится в каталоге.");

            var quantity = line.Quantity;
            var bookId = line.BookId;

            var updated = await db.Database.ExecuteSqlInterpolatedAsync(
                $"UPDATE dbo.Remains SET [Number of copies] = [Number of copies] - {quantity} WHERE ID_Shop = {shopId} AND ID_Book = {bookId} AND [Number of copies] >= {quantity}",
                ct);

            if (updated == 0)
                return Result<int>.Fail($"Недостаточно экземпляров книги «{line.Title}» в магазине. Обновите список и проверьте остаток.");

            cheque.ChequeBooks.Add(new ChequeBook { IdBook = bookId, Count = quantity, Price = book.Price });
        }

        cheque.TotalSum = cheque.ChequeBooks.Sum(x => x.Price * x.Count);
        db.Cheques.Add(cheque);
        await db.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);
        return Result<int>.Ok(cheque.IdCheque);
    }
}
