using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared;

public class ActivateUserDto
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


  [Required(ErrorMessage = "Verificación Términos y condiciones es requerido")]
  [DeniedValues(new object[] { false }, ErrorMessage = "Verificación Términos y condiciones es requerido")]
  public required bool TermsConditions { get; set; }

  [Required(ErrorMessage = "Verificación Política de privacidad es requerido")]
  [DeniedValues(new object[] { false }, ErrorMessage = "Verificación Política de privacidad es requerido")]
  public required bool PrivacyPolicy { get; set; }

}