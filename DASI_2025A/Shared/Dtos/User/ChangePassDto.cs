using System.ComponentModel.DataAnnotations;

namespace Shared;

public class ChangePassDto
{
  [Required(ErrorMessage = "Contraseña antigua requerida.")]
  public string? OldPassword { get; set; }

  [Required(ErrorMessage = "Nueva contraseña requerida.")]
  [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "La contraseña debe tener al menos 8 caracteres, una mayúscula, una minúscula, un carácter especial y un número")]
  public string? NewPassword { get; set; }

  [Required(ErrorMessage = "Confirmar nueva contraseña requerida.")]
  public string? ConfirmNewPassword { get; set; }
}

