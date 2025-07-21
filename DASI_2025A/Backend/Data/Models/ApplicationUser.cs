using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend;

public class ApplicationUser : IdentityUser
{
  [Required]
  [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s'-]+$", ErrorMessage = "El primer nombre contiene caracteres no válidos.")]
  public required string FirstName { get; set; } = "Sin definir";

  [Required]
  [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s'-]+$", ErrorMessage = "El apellido contiene caracteres no válidos.")]
  public required string LastName { get; set; } = "Sin definir";

  private DateTime _dateOfBirth;

  [Required]
  public DateTime DateOfBirth
  {
    get => _dateOfBirth;
    set
    {
      if (value > DateTime.Now)
        throw new ValidationException("La fecha de nacimiento no puede ser mayor a la fecha actual.");
      _dateOfBirth = value;
    }
  }

  [Required]
  public required string ScoutUniqueId { get; set; }

  [Required]
  [EmailAddress]
  [PersonalData]
  public override string? Email
  {
    get => base.Email;
    set
    {
      base.Email = value;
      base.UserName = value; // Establecer el email como UserName
    }
  }

  [Required]
  [Column(TypeName = "decimal(18,2)")]
  [Range(-10, 999999.99, ErrorMessage = "El saldo debe estar entre 0 y 999.999,99")]
  public decimal Balance { get; set; } = 0;

  [Required]
  public bool Active { get; set; }

  // FK a Occupation
  [Required]
  public int OccupationFk { get; set; }

  [ForeignKey("OccupationFk")]
  public OccupationEntity? Occupation { get; set; }

  // FK a Branch
  [Required]
  public int BranchFk { get; set; }

  [ForeignKey("BranchFk")]
  public BranchEntity? Branch { get; set; }

  public override string ToString()
  {
    return $"{FirstName} {LastName} ({Email}) {DateOfBirth} {ScoutUniqueId} {Balance} {Active} {OccupationFk} {BranchFk} {Id} {UserName} ";
  }
}
