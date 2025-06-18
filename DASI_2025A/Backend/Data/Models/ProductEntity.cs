using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared;

namespace Backend;
public class ProductEntity : AuditableEntity
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }

  [Required]
  [StringLength(200)]
  [RegularExpression(@"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s'.,-]+$", ErrorMessage = "El nombre del producto contiene caracteres no válidos.")]
  public required string Name { get; set; }

  [StringLength(1000)]
  [RegularExpression(@"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s'.,-]*$", ErrorMessage = "La descripción del producto contiene caracteres no válidos.")]
  public string Description { get; set; } = "Ninguna descripción disponible.";

  [Required]
  [Column(TypeName = "decimal(18,2)")]
  public decimal Price { get; set; } = 0;

  [Required]
  [Range(0, uint.MaxValue, ErrorMessage = "El stock debe ser un número positivo.")]
  [RegularExpression(@"^\d+$", ErrorMessage = "El stock debe ser un número entero positivo.")]
  public uint Stock { get; set; } = 0;

  [StringLength(500)]
  public string Image { get; set; } = "No hay imagen disponible.";

  public bool Active { get; set; } = true;

  [Required]
  public ProductType Type { get; set; }

  public string ImageDeleteHash { get; set; } = "No hay hash de eliminación disponible.";
}
