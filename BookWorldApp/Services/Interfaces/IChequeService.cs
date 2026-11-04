namespace BookWorldApp.Services;

public interface IChequeService
{
    Task<ChequeInfo?> GetAsync(int chequeId, CancellationToken ct = default);
}