using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared;
public class UserDto
{
  public string? Id { get; set; }

  [Required]
  [MaxLength(100)]
  public string FirstName { get; set; } = default!;

  [Required]
  [MaxLength(100)]
  public string LastName { get; set; } = default!;

  [Required]
  [DataType(DataType.Date)]
  public DateTime DateOfBirth { get; set; }

  [Required]
  [MaxLength(50)]
  public string ScoutUniqueId { get; set; } = default!;

    [Required]
    public decimal AccountBalance { get; set; }

  [Required]
  [EmailAddress]
  public string? Email { get; set; }

  [Required]
  public string? Password { get; set; }

  [Required]
  public bool Active { get; set; }

  [Required]
  public string? Role { get; set; }

  [Required]
  public int OccupationFk { get; set; }

  public OccupationDto? Occupation { get; set; }
}