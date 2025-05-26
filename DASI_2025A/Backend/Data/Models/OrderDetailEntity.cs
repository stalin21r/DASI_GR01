using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend;

public class OrderDetailEntity
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }

  // FK to Order
  [Required]
  public int OrderId { get; set; }

  [ForeignKey("OrderId")]
  public OrderEntity? Order { get; set; }

  // FK to Product
  [Required]
  public int ProductId { get; set; }

  [ForeignKey("ProductId")]
  public ProductEntity? Product { get; set; }

  [Required]
  public uint Quantity { get; set; }

  [Required]
  [Column(TypeName = "decimal(18,2)")]
  public decimal UnitPrice { get; set; }

  [Required]
  [Column(TypeName = "decimal(18,2)")]
  public decimal Subtotal { get; set; }
}
