using BookWorldApp.Data;
using BookWorldApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BookWorldApp.Services;

public interface IReplenishmentService
{
    Task<Result<int>> CreateAsync(BookCart cart, CancellationToken ct = default);
}

public sealed class ReplenishmentService(IDbContextFactory<BookStoreContext> factory, UserSession session) : IReplenishmentService
{
    public async Task<Result<int>> CreateAsync(BookCart cart, CancellationToken ct = default)
    {
        var user = session.RequireCashier();

        if (cart.IsEmpty) return Result<int>.Fail("Добавьте в заказ хотя бы одну книгу.");
        if (cart.FirstError() is { } error) return Result<int>.Fail(error);

        await using var db = await factory.CreateDbContextAsync(ct);

        var bookIds = cart.Lines.Select(l => l.BookId).ToList();
        var books = await db.Books.AsNoTracking().Where(b => bookIds.Contains(b.IdBook)).ToDictionaryAsync(b => b.IdBook, ct);

        var order = new Order { Date = DateTime.Now, IdShop = user.ShopId!.Value, IdEmployee = user.EmployeeId };

        foreach (var line in cart.Lines)
        {
            if (!books.TryGetValue(line.BookId, out var book))
                return Result<int>.Fail($"Книга «{line.Title}» больше не числится в каталоге.");

            order.OrderBooks.Add(new OrderBook { IdBook = book.IdBook, Count = line.Quantity, Price = book.Price });
        }

        order.Sum = order.OrderBooks.Sum(x => x.Price * x.Count);
        db.Orders.Add(order);
        await db.SaveChangesAsync(ct);
        return Result<int>.Ok(order.IdOrder);
    }
}
