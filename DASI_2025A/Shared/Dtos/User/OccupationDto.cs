using System.ComponentModel.DataAnnotations;

namespace Shared;
public class OccupationDto
{
  public int? Id { get; set; }

  [Required]
  [MaxLength(50)]
  public string? Name { get; set; }

}

