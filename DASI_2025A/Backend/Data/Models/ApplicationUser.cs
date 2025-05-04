using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        // ----- Datos propios -----
        [Required]
		public string FirstName { get; set; } = string.Empty;
		[Required]
		public string LastName { get; set; } = string.Empty;
		[Required]
		public DateTime DateOfBirth { get; set; }
		[Required]
		public string Cedula { get; set; } = string.Empty;
		[Required]
		public string UniqueScoutId { get; set; } = string.Empty;
		[Required]
		[EmailAddress]
		[PersonalData]
		public override string? Email
		{
			get => base.Email;
			set
			{
				base.Email = value;
				base.UserName = value; // Establecer el email como UserName
			}
		}

		[Required]
		public bool Active { get; set; }
		// FK a Occupation
		[Required]
		public int OccupationFk { get; set; }

		[ForeignKey("OccupationFk")]
		public OccupationEntity? Occupation { get; set; }
	}
}
