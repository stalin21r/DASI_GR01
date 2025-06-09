using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend;

public class TopUpRequestEntity : AuditableEntity
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }

  // Monto de la recarga
  [Required]
  [Column(TypeName = "decimal(18,2)")]
  public decimal Amount { get; set; }

  // Tipo de recarga: por defecto 'Efectivo'
  [Required]
  [MaxLength(50)]
  public string Type { get; set; } = "Efectivo";

  // Estado de la recarga: Pendiente, Aprobado, Rechazado
  [Required]
  [MaxLength(20)]
  public string Status { get; set; } = "PENDIENTE";

  // Comprobante opcional
  [MaxLength(255, ErrorMessage = "La descripcion debe tener menos de 255 caracteres")]
  public string? Receipt { get; set; }

  // Usuario beneficiario (al que se le recarga el saldo)
  [Required]
  public required string TargetUserId { get; set; }

  [ForeignKey("TargetUserId")]
  public ApplicationUser? TargetUser { get; set; }

  // Usuario que crea la solicitud de recarga
  [Required]
  public required string RequestedByUserId { get; set; }

  [ForeignKey("RequestedByUserId")]
  public ApplicationUser? RequestedByUser { get; set; }

  // Usuario que autoriza o revisa la solicitud (puede ser null si está pendiente)
  public string? AuthorizedByUserId { get; set; }

  [ForeignKey("AuthorizedByUserId")]
  public ApplicationUser? AuthorizedByUser { get; set; }
}
