using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ProductLoggerDto
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Action es requerido")]
        [StringLength(10, ErrorMessage = "Action debe tener menos de 10 caracteres")]
        public string Action { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "La descripcion debe tener menos de 1000 caracteres")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "QuantityBefore es requerido")]
        [Range(0, int.MaxValue, ErrorMessage = "QuantityBefore debe ser mayor o igual a 0")]
        public uint QuantityBefore { get; set; }

        [Required(ErrorMessage = "QuantityAfter es requerido")]
        [Range(0, int.MaxValue, ErrorMessage = "QuantityAfter debe ser mayor o igual a 0")]
        public uint QuantityAfter { get; set; }
    }
}
