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
<<<<<<< HEAD
        [StringLength(10)] // Debe inicializar con un valor
=======
        [StringLength(50)] // Debe inicializar con un valor
>>>>>>> origin/development
        public required string Action { get; set; }

        [Required]
        [StringLength(1000)] // Debe inicializar con un valor
        public required string Description { get; set; }

        [Required]
<<<<<<< HEAD
        public required int QuantityBefore { get; set; }

        [Required]
        public required int QuantityAfter { get; set; }
=======
        public required uint QuantityBefore { get; set; }

        [Required]
        public required uint QuantityAfter { get; set; }
>>>>>>> origin/development

        // FK a Product
        [Required]
        public int ProductFk { get; set; }
        [ForeignKey("ProductFk")]
        public ProductEntity? Product { get; set; }

        // FK a User
        [Required]
<<<<<<< HEAD
        public int UserFk { get; set; }
=======
        public required string UserFk { get; set; }
>>>>>>> origin/development
        [ForeignKey("UserFk")]
        public ApplicationUser? User { get; set; }

    }
}
