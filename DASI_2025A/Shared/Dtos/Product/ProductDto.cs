using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Shared;

public class ProductDto
{
  public int Id { get; set; }

  [Required(ErrorMessage = "El nombre es requerido")]
  [StringLength(200, ErrorMessage = "El nombre debe tener menos de 200 caracteres")]
  [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9\s'-]+$", ErrorMessage = "El nombre contiene caracteres no válidos.")]
  public string Name { get; set; } = string.Empty;

  [Required(ErrorMessage = "La descripción es requerida")]
  [StringLength(1000, ErrorMessage = "La descripción debe tener menos de 1000 caracteres")]
  [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9\s,'-]+$", ErrorMessage = "La descripción contiene caracteres no válidos.")]
  public string Description { get; set; } = string.Empty;

  [Required(ErrorMessage = "El precio es requerido")]
  [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
  public decimal Price { get; set; }

  [Required(ErrorMessage = "La cantidad es requerida")]
  [Range(0, uint.MaxValue, ErrorMessage = "La cantidad debe ser mayor o igual a 0")]
  public uint Stock { get; set; } = 0;

  public string? Image { get; set; } = "Imagen no disponible";

  [Required(ErrorMessage = "El estado es requerido")]
  public bool Active { get; set; } = true;

  [Required(ErrorMessage = "El tipo es requerido")]
  public ProductType Type { get; set; }


  public override string ToString()
  {
    return $"Id: {Id}, Name: {Name}, Description: {Description}, Price: {Price}, Stock: {Stock}, Active: {Active}, Type: {Type}";
  }

}
