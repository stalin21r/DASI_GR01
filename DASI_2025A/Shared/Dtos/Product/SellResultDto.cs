using System;

namespace Shared;

public class SellResultDto
{
	public ProductDto Product { get; set; } = null!;

	public OrderDto Order { get; set; } = null!;

	public WalletDto WalletTransaction { get; set; } = null!;

	public DateTime TransactionDate { get; set; } = DateTime.Now;

	public decimal UpdatedBalance { get; set; }
}