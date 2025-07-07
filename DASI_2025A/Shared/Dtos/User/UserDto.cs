﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared;

public class UserDto
{
  public string? Id { get; set; }

  [Required(ErrorMessage = "El nombre es obligatorio")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres")]
  [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios")]
  public string FirstName { get; set; } = default!;

  [Required(ErrorMessage = "El apellido es requerido.")]
  [StringLength(100, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 100 caracteres")]
  [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El apellido solo puede contener letras y espacios")]
  public string LastName { get; set; } = default!;

  [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
  [DataType(DataType.Date)]
  
  public DateTime DateOfBirth { get; set; }

  [Required(ErrorMessage = "El número único es requerido.")]
  [MaxLength(50)]
  public string ScoutUniqueId { get; set; } = default!;

  [Required(ErrorMessage = "El email es obligatorio")]
  [EmailAddress(ErrorMessage = "Formato de email inválido")]
  [StringLength(100, ErrorMessage = "El email no puede exceder 100 caracteres")]
  public string Email { get; set; } = default!;

  [Required(ErrorMessage = "La contraseña es obligatoria")]
  [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "La contraseña debe tener al menos 8 caracteres, una mayúscula, una minúscula, un carácter especial y un número")]
  public string? Password { get; set; }

  public bool Active { get; set; } = true;

  public decimal Balance { get; set; }

  [Required(ErrorMessage = "El rol es requerido")]
  public string? Role { get; set; }

  [Required(ErrorMessage = "La ocupación es requerida")]
  public int OccupationFk { get; set; }

  public OccupationDto? Occupation { get; set; }

  public override bool Equals(object? obj)
  {
    return obj is UserDto dto && Id == dto.Id;
  }

  public override int GetHashCode()
  {
    return Id != null ? Id.GetHashCode() : 0;
  }
}