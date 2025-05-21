using System.ComponentModel.DataAnnotations;

namespace Shared;
public class WalletDto
{
	public int Id { get; set; }
	[Required(ErrorMessage = "La acción es requerida")]
	public string Action { get; set; }
	[Required(ErrorMessage = "El estado es requerido")]
	public string Status { get; set; }
	[Range(-10, 1000)]
	public decimal Amount { get; set; }
	public int? OrderId { get; set; }
	public DateTime AuditableDate { get; set; }
	public string UserId { get; set; } = null!;
}