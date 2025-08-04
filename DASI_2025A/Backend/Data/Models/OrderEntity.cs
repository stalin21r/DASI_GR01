using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend;
public class OrderEntity : AuditableEntity
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	[Required]
	public required string OrderNote { get; set; }
	[Required]
	public required DateTime OrderDate { get; set; }
	[Required]
	[Column(TypeName = "decimal(18,2)")]
	public required decimal TotalAmount { get; set; }
	[Required]
	public string? Status { get; set; } = null!;
	[Required]
	public string UserId { get; set; } = null!;
	[ForeignKey("UserFk")]
	public ApplicationUser? User { get; set; }
	public List<OrderDetailEntity> Details { get; set; } = new List<OrderDetailEntity>();
}