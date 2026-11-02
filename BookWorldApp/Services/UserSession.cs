namespace BookWorldApp.Services;

public static class Roles
{
    public const string Cashier = "Cashier";
    public const string Admin = "Admin";

    public static string Display(string role) => role switch
    {
        Cashier => "Кассир-продавец",
        Admin => "Администратор",
        _ => role
    };
}

public sealed record CurrentUser(int EmployeeId, string Login, string Role, int? ShopId, string? ShopName);

public sealed class UserSession
{
    public CurrentUser? User { get; private set; }

    public void SignIn(CurrentUser user) => User = user;

    public void SignOut() => User = null;

    public CurrentUser RequireCashier()
    {
        var user = User ?? throw new UnauthorizedAccessException("Требуется авторизация.");
        if (user.Role != Roles.Cashier || user.ShopId is null)
            throw new UnauthorizedAccessException("Операция доступна только кассиру-продавцу.");
        return user;
    }
}
