namespace BookWorldApp.Services;

public interface IReferenceService
{
    Task<ReferenceData> GetAsync(CancellationToken ct = default);
}