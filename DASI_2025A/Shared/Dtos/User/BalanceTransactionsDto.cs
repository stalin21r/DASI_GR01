using System.ComponentModel.DataAnnotations;
namespace Shared;

public class BalanceTransactionDto
{
  public int Id { get; set; }
  public decimal Amount { get; set; }
  public string Type { get; set; } = string.Empty; // credit or debit
  [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s'-]+$", ErrorMessage = "La descripción contiene caracteres no válidos.")]
  public string? Description { get; set; }
  public decimal BalanceAfter { get; set; }
  public DateTime AuditableDate { get; set; }
}