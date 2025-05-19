using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Backend
{
    public class OrderDetailEntity : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal SubTotal { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))] // Revisar si es el nombre correcto
        public OrderEntity Order { get; set; } = null!;

        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public ProductEntity Product { get; set; } = null!;

        // OrderId y ProductId pueden tener ids duplicados

    }
}
