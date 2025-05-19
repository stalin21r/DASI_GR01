using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Backend
{
    public class WalletEntity : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; } = 0m;

        public bool IsActive { get; set; } = true;

        public string UserId { get; set; } = null!;
        [ForeignKey("UserFk")]
        public required ApplicationUser User { get; set; }
    }
}
