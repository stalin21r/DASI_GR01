namespace Shared;

public class OrderDetailDto
{
	public int Id { get; set; }
	public uint Quantity { get; set; }
	public decimal UnitPrice { get; set; }
	public int OrderId { get; set; }
	public int ProductId { get; set; }
	public ProductDto? Product { get; set; }
}
