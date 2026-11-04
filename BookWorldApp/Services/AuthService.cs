using BookWorldApp.Data;
using Microsoft.EntityFrameworkCore;

namespace BookWorldApp.Services;

public class AuthService : IAuthService
{
    private readonly IDbContextFactory<BookStoreContext> _factory;
    private readonly UserSession _session;

    public const string InvalidCredentials = "Неверный логин или пароль. Проверьте данные и повторите ввод.";

    public const string EmptyCredentials = "Введите логин и пароль.";

    public AuthService(
        IDbContextFactory<BookStoreContext> factory,
        UserSession session)
    {
        _factory = factory;
        _session = session;
    }

    public async Task<Result<CurrentUser>> LoginAsync(
        string? login,
        string? password,
        CancellationToken ct = default)
    {
        login = login?.Trim();

        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
        {
            return Result<CurrentUser>.Fail(EmptyCredentials);
        }

        using var db = await _factory.CreateDbContextAsync(ct);

        var employee = await db.Employees
            .AsNoTracking()
            .Include(e => e.Shop)
            .FirstOrDefaultAsync(e => e.Login == login, ct);

        if (employee == null ||
            !string.Equals(password, employee.Password, StringComparison.Ordinal))
        {
            return Result<CurrentUser>.Fail(InvalidCredentials);
        }

        string? shopAddress = null;

        if (employee.Shop != null)
        {
            shopAddress = $"{employee.Shop.City}, ул. {employee.Shop.Street}, д. {employee.Shop.House}";
        }

        var user = new CurrentUser(
            employee.IdEmployee,
            employee.Login,
            employee.Role,
            employee.IdShop,
            shopAddress);

        _session.SignIn(user);

        return Result<CurrentUser>.Ok(user);
    }
}