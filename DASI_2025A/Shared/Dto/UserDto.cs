using System.ComponentModel.DataAnnotations;

namespace Shared;

public class UserDto
{
	[Required(ErrorMessage = "El nombre es requerido"), StringLength(200)]
	public string FirstName { get; set; } = string.Empty;
	[Required(ErrorMessage = "El apellido es requerido"), StringLength(200)]
	public string LastName { get; set; } = string.Empty;
	[Required(ErrorMessage = "La fecha de nacimiento es requerida")]
	public DateTime DateOfBirth { get; set; }
	[Required]
	public string Cedula { get; set; } = string.Empty;
	[Required]
	public string UniqueScoutId { get; set; } = string.Empty;
	[Required]
	[EmailAddress]
	public string? Email { get; set; }
	[Required(ErrorMessage = "El estado es requerido")]
	public bool Active { get; set; }
	[Required]
	public String? Occupation { get; set; }
}
