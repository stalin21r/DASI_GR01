using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Backend
{
    public class WalletTransactionEntity : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Importe (+) abona saldo, (–) descuenta
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        // Clasificación de la transaccion
        public TransactionType Type { get; set; }   // enum: Credit, Debit, Refund

        [StringLength(250)]
        public string? Description { get; set; }

        // Snapshot del saldo tras la operación
        [Column(TypeName = "decimal(18,2)")]
        public decimal BalanceAfter { get; set; }

        // Datos opcionales para rastrear su origen
        public string? ReferenceEntity { get; set; } // p.ej. "Order", "TopUp"
        public int? ReferenceId { get; set; } // Id en esa entidad

        // Relación con la billetera
        public int WalletId { get; set; }
        [ForeignKey(nameof(WalletId))]
        public required WalletEntity Wallet { get; set; }

        // Reversiones / anulaciones
        public bool IsReverted { get; set; } = false;
        public int? ParentTransactionId { get; set; }
        [ForeignKey(nameof(ParentTransactionId))]
        public WalletTransactionEntity? ParentTransaction { get; set; }


    }
}
