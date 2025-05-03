using Microsoft.AspNetCore.Identity;

namespace Backend.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        // ----- Datos propios -----
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Cedula { get; set; } = string.Empty;
        public string UniqueScoutId { get; set; } = string.Empty;
    }
}
