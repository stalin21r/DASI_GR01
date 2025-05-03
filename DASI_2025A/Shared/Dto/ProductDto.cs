﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Shared;

public class ProductDto
{
  public int Id { get; set; }

  [Required(ErrorMessage = "El nombre es requerido")]
  [StringLength(200, ErrorMessage = "El nombre debe tener menos de 200 caracteres")]
  public string Name { get; set; } = string.Empty;

  [StringLength(1000, ErrorMessage = "La descripción debe tener menos de 1000 caracteres")]
  public string Description { get; set; } = string.Empty;

  [Required(ErrorMessage = "El precio es requerido")]
  [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
  public decimal Price { get; set; }

  [StringLength(500, ErrorMessage = "La imagen debe tener menos de 500 caracteres")]
  public string? Image { get; set; }

  [Required(ErrorMessage = "El estado es requerido")]
  public bool Active { get; set; } = true;

  [Required(ErrorMessage = "El tipo es requerido")]
  public ProductType Type { get; set; }
}
