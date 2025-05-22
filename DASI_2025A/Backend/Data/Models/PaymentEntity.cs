using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Shared;
using Microsoft.EntityFrameworkCore;

namespace Backend
{
    public class PaymentEntity : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Precision(18, 4)]
        public decimal AmountDue { get; set; } // Monto a pagar

        [Required]
        public PaymentMethod PaymentMethod { get; set; } // Metodo de pago

        [Required]
        public Status Status { get; set; } = Status.Pending;

        public DateTime? PaidAt { get; set; } // Fecha de pago

        public string? ComprobanteUrl { get; set; }

        // Reversiones / anulaciones
        // public bool IsReverted { get; set; } = false;
        public int? ParentOrderId { get; set; }
        [ForeignKey(nameof(ParentOrderId))]
        public OrderEntity? ParentOrder { get; set; }
    }
}
