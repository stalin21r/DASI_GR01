using System.ComponentModel.DataAnnotations;

namespace Shared;

public class RecoverPassDto
{
  [Required(ErrorMessage = "Correo requerido.")]
  public required string Email { get; set; }

  [Required(ErrorMessage = "Token requerido.")]
  public required string Token { get; set; }

  [Required(ErrorMessage = "Nueva contraseña requerida.")]
  [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "La contraseña debe tener al menos 8 caracteres, una mayúscula, una minúscula, un carácter especial y un número")]
  public string? NewPassword { get; set; }

  [Required(ErrorMessage = "Confirmar nueva contraseña requerida.")]
  public string? ConfirmNewPassword { get; set; }
}

