using System;

namespace Shared;

public class SellResultDto
{
	public ProductDto Product { get; set; } = null!;
	public DateTime TransactionDate { get; set; } = DateTime.Now;
	public decimal UpdatedBalance { get; set; }
}