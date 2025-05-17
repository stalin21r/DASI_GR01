namespace Backend;
	public interface IWalletRepository
	{
		Task<WalletEntity> GetByIdAsync(int id);
		Task<List<WalletEntity>> GetByOrderIdAsync(int orderId);
		Task<List<WalletEntity>> GetByUserIdAsync(string userId);
		Task<WalletEntity> CreateAsync(WalletEntity wallet);
	}
