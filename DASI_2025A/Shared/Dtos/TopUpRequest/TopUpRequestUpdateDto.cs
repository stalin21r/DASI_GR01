using System.ComponentModel.DataAnnotations;

namespace Shared;

public class TopUpRequestUpdateDto
{
  [Required]
  public int Id { get; set; }
  // Solo permitimos actualizar el estado y el autorizador
  [Required]
  [MaxLength(20)]
  public string? Status { get; set; } // Pendiente, Aprobado, Rechazado
  public string? AuthorizedByUserId { get; set; }
}