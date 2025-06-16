using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Shared;

namespace Backend
{
    public class ProductLoggerEntity : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)] // Debe inicializar con un valor
        public required string Action { get; set; }

        [Required]
        [StringLength(1000)] // Debe inicializar con un valor
        [RegularExpression(@"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s'.,-]*$", ErrorMessage = "La descripción contiene caracteres no válidos.")]
        public required string Description { get; set; } = "Ninguna descripción disponible.";

        [Required]
        public required uint QuantityBefore { get; set; }

        [Required]
        public required uint QuantityAfter { get; set; }

        // FK a Product
        [Required]
        public int ProductFk { get; set; }
        [ForeignKey("ProductFk")]
        public ProductEntity? Product { get; set; }

        // FK a User
        [Required]
        public required string UserFk { get; set; }
        [ForeignKey("UserFk")]
        public ApplicationUser? User { get; set; }

    }
}
