using Microsoft.AspNetCore.Identity;

namespace Backend;

public class IdentityInitializer
{
  public static async Task InitializeAsync(IServiceProvider serviceProvider)
  {

    // Crear los Roles
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Superadmin", "Admin", "User" };

    foreach (var role in roles)
    {
      if (!await roleManager.RoleExistsAsync(role))
      {
        await roleManager.CreateAsync(new IdentityRole(role));
      }
    }

    // Crear un SuperAdmin
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var config = serviceProvider.GetRequiredService<IConfiguration>();
    var superadmin = config.GetSection("Superadmin");

    var superadminEmail = superadmin["Email"] ?? string.Empty;
    var superadminPassword = superadmin["Password"] ?? string.Empty;

    if (await userManager.FindByEmailAsync(superadminEmail) == null)
    {
      var user = new ApplicationUser
      {
        FirstName = "Aquiles",
        LastName = "Superadmin",
        Email = superadminEmail,
        UserName = superadminEmail,
        ScoutUniqueId = "Aquiles123",
        DateOfBirth = new DateTime(1990, 1, 1),
        OccupationFk = 1, // Jefe
        Active = true
      };

      var result = await userManager.CreateAsync(user, superadminPassword);
      if (result.Succeeded)
      {
        await userManager.AddToRoleAsync(user, "Superadmin");
      }
    }
  }

}
