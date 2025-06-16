using System.ComponentModel.DataAnnotations;

namespace Backend;

public abstract class AuditableEntity
{
  [Required]
  public DateTime AuditableDate { get; set; }
  [Required]
  [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s'-]+$", ErrorMessage = "El nombre de la máquina contiene caracteres no válidos.")]
  public string? MachineName { get; set; }
}
