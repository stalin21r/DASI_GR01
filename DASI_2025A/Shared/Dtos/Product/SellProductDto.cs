using System.ComponentModel.DataAnnotations;

namespace Shared;
public class SellProductDto
{
  public int ProductId { get; set; }

  [Required]
  [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero")]
  public uint Quantity { get; set; }
}
