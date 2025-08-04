using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Shared;

public class UpdateProductDto
{
  public int Id { get; set; }

  [Required(ErrorMessage = "El nombre es requerido")]
  [StringLength(200, ErrorMessage = "El nombre debe tener menos de 200 caracteres")]
  public string Name { get; set; } = string.Empty;

  [StringLength(1000, ErrorMessage = "La descripción debe tener menos de 1000 caracteres")]
  public string Description { get; set; } = string.Empty;

  [Required(ErrorMessage = "La cantidad es requerida")]
  [Range(0, uint.MaxValue, ErrorMessage = "La cantidad debe ser mayor o igual a 0")]
  public uint Stock { get; set; } = 0;

  public string? Image { get; set; } = "Imagen no disponible";

  [Required(ErrorMessage = "El estado es requerido")]
  public bool Active { get; set; } = true;

  [Required(ErrorMessage = "El tipo es requerido")]
  public ProductType Type { get; set; }

  [Required(ErrorMessage = "Razón de la actualización es requerida")]
  public string Reason { get; set; } = string.Empty;
}
