namespace BookWorldApp.Services;

public interface IAuthService
{
    Task<Result<CurrentUser>> LoginAsync(
        string? login,
        string? password,
        CancellationToken ct = default);
}