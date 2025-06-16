using System.ComponentModel.DataAnnotations;

namespace Shared
{
  public class ProductLoggerDto
  {
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    public required string UserId { get; set; }

    [Required(ErrorMessage = "Action es requerido")]
    [StringLength(50, ErrorMessage = "Action debe tener menos de 50 caracteres")]
    public string Action { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "La descripción debe tener menos de 1000 caracteres")]
    public string Description { get; set; } = "Descripción no proporcionada";

    [Required(ErrorMessage = "QuantityBefore es requerido")]
    [Range(0, int.MaxValue, ErrorMessage = "QuantityBefore debe ser mayor o igual a 0")]
    public uint QuantityBefore { get; set; }

    [Required(ErrorMessage = "QuantityAfter es requerido")]
    [Range(0, int.MaxValue, ErrorMessage = "QuantityAfter debe ser mayor o igual a 0")]
    public uint QuantityAfter { get; set; }
    public string? UserName { get; set; }

    public DateTime AuditableDate { get; set; }

    public string? ProductName { get; set; }

  }
}
