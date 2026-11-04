namespace BookWorldApp.Services;

public interface IAvailabilityService
{
    Task<AvailabilityReport> GetAsync( int bookId, CancellationToken ct = default);
}