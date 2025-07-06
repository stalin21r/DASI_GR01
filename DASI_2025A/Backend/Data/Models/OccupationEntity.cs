﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend;

public class OccupationEntity : AuditableEntity
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }
  [Required]
  [MaxLength(50)]
  [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s'-]+$", ErrorMessage = "El nombre contiene caracteres no válidos.")]
  public string Name { get; set; } = "No definido";
  public ICollection<ApplicationUser>? Users { get; set; }
}
