using BookWorldApp.Models;

namespace BookWorldApp.Services;

public interface ISaleService
{
    Task<Result<int>> ConfirmAsync(BookCart cart, CancellationToken ct = default);
}