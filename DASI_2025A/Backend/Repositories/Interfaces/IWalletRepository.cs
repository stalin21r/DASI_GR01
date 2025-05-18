using Shared;

namespace Backend;

public interface IWalletRepository
{
	Task<WalletDto> CreateAsync(WalletDto walletDto);
	Task<WalletDto?> GetAsync(int id);
	Task<IEnumerable<WalletDto>> GetByOrderIdAsync(int orderId);
	Task<IEnumerable<WalletDto>> GetByUserIdAsync(string userId);
	Task<decimal> GetUserBalanceAsync(string userId);
	Task<WalletDto> RegisterSaleTransactionAsync(int orderId, decimal amount, string userId);
}