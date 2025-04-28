namespace Shared;
using System.ComponentModel.DataAnnotations;

public class CombosDTO
{
  public int Id { get; set; }
  [Required(ErrorMessage = "El nombre del combo es requerido")]
  [MaxLength(100)]
  public string? Name { get; set; }
  [MaxLength(500)]
  public string? Description { get; set; }
  [Required(ErrorMessage = "El precio del combo es requerido")]
  [Range(0, float.MaxValue, ErrorMessage = "El precio del combo debe ser mayor o igual a cero")]
  public float Price { get; set; }
}
