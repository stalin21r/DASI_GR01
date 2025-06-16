using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend;

public class BalanceTransactionsEntity : AuditableEntity
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }
  [Required]
  public required string UserId { get; set; }
  [Required]
  [Column(TypeName = "decimal(18,2)")]
  public decimal Amount { get; set; }
  [Required]
  [MaxLength(50)]
  public required string Type { get; set; } //RECARGA o DÉBITO
  [MaxLength(255, ErrorMessage = "La descripcion debe tener menos de 255 caracteres")]
  [RegularExpression(@"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s'.,-]*$", ErrorMessage = "La descripción contiene caracteres no válidos.")]
  public string? Description { get; set; }
  [Required]
  [Column(TypeName = "decimal(18,2)")]
  public decimal BalanceBefore { get; set; }
  [Required]
  [Column(TypeName = "decimal(18,2)")]
  public decimal BalanceAfter { get; set; }
  [ForeignKey("UserId")]
  public ApplicationUser? User { get; set; }
}