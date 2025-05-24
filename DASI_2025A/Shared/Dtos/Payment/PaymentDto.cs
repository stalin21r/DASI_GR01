using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class PaymentDto
    {
        public int Id { get; set; }

        public decimal AmountDue { get; set; }

        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.AccountBalance;  

        public Status Status { get; set; } = Status.Pending;  

        public DateTime IssuedAt { get; set; }

        public string? ComprobanteUrl { get; set; }

        public int? OrderId { get; set; }
    }
}
