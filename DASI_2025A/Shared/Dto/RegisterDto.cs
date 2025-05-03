using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dto
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "El nombre es requerido"), StringLength(200)] 
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "El apellido es requerido"), StringLength(200)] 
        public string LastName { get; set; } = "";

        [Required(ErrorMessage = "El email es requerido"), EmailAddress] 
        public string Email { get; set; } = "";

        [Required, MinLength(6)] 
        public string Password { get; set; } = "";

        public string Cedula { get; set; } = "";
    }
}
