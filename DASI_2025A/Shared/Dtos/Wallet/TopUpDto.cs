using System.ComponentModel.DataAnnotations;

namespace Shared;

public class TopUpDto
{
	[Required]
	[Range(0.01, 10000, ErrorMessage = "El monto debe estar entre 0.01 y 10000")]
	public decimal Amount { get; set; }

	public string? Description { get; set; }
}