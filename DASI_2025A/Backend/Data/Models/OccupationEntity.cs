using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared;

namespace Backend;
public class OccupationEntity : AuditableEntity
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }
  [Required]
  [MaxLength(50)]
  public string? Name { get; set; }

  public ICollection<ApplicationUser>? Users { get; set; }
}
