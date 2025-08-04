using Shared;

namespace Backend;

public interface IWalletService
{
	Task<decimal> GetBalanceAsync(string userId);
	Task<IEnumerable<WalletDto>> GetTransactionHistoryAsync(string userId);
	Task<WalletDto> TopUpBalanceAsync(decimal amount, string userId, string description = "Recarga de saldo");
	Task<WalletDto?> GetTransactionAsync(int id);
}