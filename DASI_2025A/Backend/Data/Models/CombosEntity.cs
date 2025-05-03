using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend;

public class CombosEntity
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }

  [Required]
  [MaxLength(100)]
  public string? Name { get; set; }

  [MaxLength(500)]
  public string? Description { get; set; }

  [Required]
  [Range(0, float.MaxValue)]
  public float Price { get; set; }
}
