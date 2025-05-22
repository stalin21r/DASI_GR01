using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Shared
{
    public class OrderDto
    {
        public int? Id { get; set; }
        public string OrderNote { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public Status Status { get; set; } = Status.Pending;
        public bool? IsActive { get; set; } = true;
        public bool? IsReverted { get; set; } = false;
        public string UserId { get; set; } = string.Empty;

        public List<OrderDetailDto> Details { get; set; } = new(); // DTO anidado para detalles

        //El DTO de Order debe recibir:
        //    OrderNote
        //    OrderDate
        //    TotalAmount
        //    UserId
    }
}
