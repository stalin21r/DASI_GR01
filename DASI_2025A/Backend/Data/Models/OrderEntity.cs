using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend;

public class OrderEntity : AuditableEntity
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }

  // FK: User who buys
  [Required]
  public string BuyerId { get; set; } = null!;

  [ForeignKey("BuyerId")]
  public ApplicationUser? Buyer { get; set; }

  // FK: User who sells
  [Required]
  public string SellerId { get; set; } = null!;

  [ForeignKey("SellerId")]
  public ApplicationUser? Seller { get; set; }

  [Required]
  [Column(TypeName = "decimal(18,2)")]
  public decimal Total { get; set; }

  public ICollection<OrderDetailEntity> OrderDetails { get; set; } = new List<OrderDetailEntity>();
}
