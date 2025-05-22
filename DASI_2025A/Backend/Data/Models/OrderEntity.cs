using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared;

namespace Backend
{
    public class OrderEntity : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public required string OrderNote { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public required decimal TotalAmount { get; set; }

         public Status Status { get; set; } = Status.Pending; // Tipo enum OrderStatus

        public bool IsActive { get; set; } = true;

        [Required]
        public string UserId { get; set; } = null!;
        [ForeignKey("UserFk")]
        public ApplicationUser? User { get; set; }

        // Reversiones / anulaciones
        public bool IsReverted { get; set; } = false;

        public List<OrderDetailEntity> Details { get; set; } = new List<OrderDetailEntity>();

    }
}
