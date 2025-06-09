using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared;

public class UserDto
{
  public string? Id { get; set; }

  [Required(ErrorMessage = "El nombre es requerido.")]
  [MaxLength(100)]
  public string FirstName { get; set; } = default!;

  [Required(ErrorMessage = "El apellido es requerido.")]
  [MaxLength(100)]
  public string LastName { get; set; } = default!;

  [Required(ErrorMessage = "La fecha de nacimiento es requerida.")]
  [DataType(DataType.Date)]
  public DateTime DateOfBirth { get; set; }

  [Required(ErrorMessage = "El número único es requerido.")]
  [MaxLength(50)]
  public string ScoutUniqueId { get; set; } = default!;

  [Required(ErrorMessage = "El correo es requerido.")]
  [EmailAddress]
  public string? Email { get; set; }

  [Required(ErrorMessage = "La contraseña es requerida")]
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