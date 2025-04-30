using System.ComponentModel.DataAnnotations;

namespace Backend;

public abstract class AuditableEntity
{
  [Required]
  public DateTime AuditableDate { get; set; }
  [Required]
  public string? MachineName { get; set; }
}
