namespace Shared;

public class OrderDto
{
	public int Id { get; set; }
	public string OrderNote { get; set; }
	public DateTime OrderDate { get; set; }
	public decimal TotalAmount { get; set; }
	public string Status { get; set; }
	public string UserId { get; set; }
	public List<OrderDetailDto> Details { get; set; } = new();
}
