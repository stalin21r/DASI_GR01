
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend;

public class OrderDetailEntity : AuditableEntity
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	[Required]
	public uint Quantity { get; set; }
	[Required]
	[Column(TypeName = "decimal(18,2)")]
	public decimal UnitPrice { get; set; }
	[ForeignKey("OrderFk")]
	public int OrderId { get; set; }
	public OrderEntity? Order { get; set; }
	[ForeignKey("ProductFk")]
	public int ProductId { get; set; }
	public ProductEntity? Product { get; set; }
}
