using System.ComponentModel.DataAnnotations;

namespace Shared;

public class LoginDto
{
  [Required(ErrorMessage = "* El email es requerido")]
  [EmailAddress(ErrorMessage = "* El email no es válido")]
  public string Email { get; set; } = default!;

  [Required(ErrorMessage = "* La contraseña es requerida")]
  public string Password { get; set; } = default!;
}
