namespace Shared;

public class OrderDetailResponseDto
{
  public int Id { get; set; }

  public int ProductId { get; set; }

  public string? ProductName { get; set; }

  public uint Quantity { get; set; }

  public decimal UnitPrice { get; set; }

  public decimal Subtotal { get; set; }
}
