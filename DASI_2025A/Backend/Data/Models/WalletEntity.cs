using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend;

public class WalletEntity : AuditableEntity
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	[Required]
	public string Action { get; set; } = null!;
	[Required]
	public string? Status { get; set; } = null!;
	[Required]
	[Column(TypeName = "decimal(18,2)")]
	[Range(-10, 1000)]
	public decimal Amount { get; set; } // Amount of money added or removed	
	[ForeignKey("OrderFk")]
	public int OrderId { get; set; }
	public OrderEntity? Order { get; set; }
	public string UserId { get; set; } = null!;
}
