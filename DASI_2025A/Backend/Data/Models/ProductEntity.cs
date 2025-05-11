using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared;

namespace Backend;
public class ProductEntity : AuditableEntity
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }

  [Required]
  [StringLength(200)]
  public required string Name { get; set; }

  [StringLength(1000)]
  public string? Description { get; set; }

  [Required]
  [Column(TypeName = "decimal(18,2)")]
  public decimal Price { get; set; }
	
  [Required]
  public uint Stock { get; set; }

  [StringLength(500)]
  public string? Image { get; set; }

  public bool Active { get; set; } = true;

  [Required]
  public ProductType Type { get; set; } 

}
