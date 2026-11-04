namespace BookWorldApp.Services
{
    public interface IReplenishmentService
    {
        Task<Result<int>> CreateAsync(BookCart cart, CancellationToken ct = default);
    }
}
