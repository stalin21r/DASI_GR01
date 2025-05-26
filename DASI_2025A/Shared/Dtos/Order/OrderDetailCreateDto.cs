using System.ComponentModel.DataAnnotations;

namespace Shared;

public class OrderDetailCreateDto
{
  [Required]
  public int ProductId { get; set; }
  public string? ProductName { get; set; }
  [Required]
  public uint Quantity { get; set; }
  [Required]
  public decimal UnitPrice { get; set; }
  [Required]
  public decimal Subtotal { get; set; }
}
