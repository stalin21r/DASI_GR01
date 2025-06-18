using System.ComponentModel.DataAnnotations;

namespace Shared;

public class OrderCreateDto
{
  [Required]
  public string BuyerId { get; set; } = null!;

  [Required]
  public string SellerId { get; set; } = null!;

  [Required]
  
  public decimal Total { get; set; }

  [Required]
  public List<OrderDetailCreateDto> OrderDetails { get; set; } = new();
}
