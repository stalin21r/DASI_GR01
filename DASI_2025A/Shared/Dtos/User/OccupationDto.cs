using System.ComponentModel.DataAnnotations;

namespace Shared;

public class OccupationDto
{
  public int? Id { get; set; }

  [Required]
  [MaxLength(50)]
  [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios.")]
  public string? Name { get; set; }

}

