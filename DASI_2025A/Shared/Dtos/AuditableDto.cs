using System.ComponentModel.DataAnnotations;

namespace Shared;

public class AuditableDto
{
  [Required]
  public DateTime AuditableDate { get; set; }
  [Required]
  public string? MachineName { get; set; }
}
