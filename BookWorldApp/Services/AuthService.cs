using BookWorldApp.Data;
using Microsoft.EntityFrameworkCore;

namespace BookWorldApp.Services;

public interface IAuthService
{
    Task<Result<CurrentUser>> LoginAsync(string? login, string? password, CancellationToken ct = default);
}

public sealed class AuthService(IDbContextFactory<BookStoreContext> factory, UserSession session) : IAuthService
{
    public const string InvalidCredentials = "Неверный логин или пароль. Проверьте данные и повторите ввод.";
    public const string EmptyCredentials = "Введите логин и пароль.";

    public async Task<Result<CurrentUser>> LoginAsync(string? login, string? password, CancellationToken ct = default)
    {
        login = login?.Trim();
        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            return Result<CurrentUser>.Fail(EmptyCredentials);

        await using var db = await factory.CreateDbContextAsync(ct);
        var employee = await db.Employees.AsNoTracking()
            .Include(e => e.Shop)
            .FirstOrDefaultAsync(e => e.Login == login, ct);

        if (employee is null || !string.Equals(password, employee.Password, StringComparison.Ordinal))
            return Result<CurrentUser>.Fail(InvalidCredentials);

        var shopName = employee.Shop is null ? null : $"{employee.Shop.City}, ул. {employee.Shop.Street}, д. {employee.Shop.House}";
        var user = new CurrentUser(employee.IdEmployee, employee.Login, employee.Role, employee.IdShop, shopName);
        session.SignIn(user);
        return Result<CurrentUser>.Ok(user);
    }
}
