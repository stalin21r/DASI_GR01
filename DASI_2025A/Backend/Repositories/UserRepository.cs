using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Backend;
public class UserRepository : IUserRepository
{
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly RoleManager<IdentityRole> _roleManager;
  private readonly ApplicationDbContext _context;

  public UserRepository(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    ApplicationDbContext context
    )
  {
    _userManager = userManager;
    _roleManager = roleManager;
    _context = context;
  }
  public async Task<bool> AssignRoleAsync(ApplicationUser user, string roleName)
  {
    if (!string.IsNullOrEmpty(roleName))
    {
      var result = await _userManager.AddToRoleAsync(user, roleName);
      if (!result.Succeeded)
      {
        await _userManager.DeleteAsync(user);
        throw new BadHttpRequestException("Error al crear usuario, no se pudo asignar el rol.");
      }
      return true;
    }
    else
    {
      throw new ArgumentException("El rol es requerido.");
    }
  }


  public async Task<UserDto> CreateAsync(UserDto userDto)
  {
    var user = new ApplicationUser
    {
      Email = userDto.Email,
      FirstName = userDto.FirstName,
      LastName = userDto.LastName,
      DateOfBirth = userDto.DateOfBirth,
      ScoutUniqueId = userDto.ScoutUniqueId,
      Active = userDto.Active,
      OccupationFk = userDto.OccupationFk,
    };

    if (string.IsNullOrEmpty(userDto.Password))
    {
      throw new ArgumentException("Password cannot be null or empty.", nameof(userDto.Password));
    }
    var result = await _userManager.CreateAsync(user, userDto.Password);
    if (!result.Succeeded)
    {
      throw new BadHttpRequestException("Error al crear el nuevo usuario.");
    }
    var roleResult = await AssignRoleAsync(user, userDto.Role ?? "User");
    if (!roleResult)
    {
      await _userManager.DeleteAsync(user);
      throw new BadHttpRequestException("Error al crear el nuevo usuario, no se pudo asignar el rol.");
    }
    userDto.Id = user.Id;
    OccupationEntity? occupation = user.Occupation;
    userDto.Occupation = occupation != null ? new OccupationDto
    {
      Id = occupation.Id,
      Name = occupation.Name
    } : null;
    return userDto;
  }

  public async Task<IEnumerable<UserDto>> GetAllAsync()
  {
    var users = await _userManager.Users
    .Include(u => u.Occupation)
    .AsNoTracking()
    .ToListAsync();
    if (users == null || users.Count == 0)
    {
      throw new KeyNotFoundException("No se encontraron Usuarios.");
    }
    var userDtos = new List<UserDto>();
    // se pre carga todos los roles para no hacer muchas consultas
    var userRoles = new Dictionary<string, IList<string>>();
    foreach (var user in users)
    {
      userRoles[user.Id] = await _userManager.GetRolesAsync(user);
    }
    // Mapear los usuarios de entidad a dto´s
    foreach (var user in users)
    {
      var roles = userRoles[user.Id];
      var userDto = new UserDto
      {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Email = user.Email,
        DateOfBirth = user.DateOfBirth,
        ScoutUniqueId = user.ScoutUniqueId,
        Active = user.Active,
        Role = roles.FirstOrDefault(), // Tomamos el primer rol si hay múltiples
        OccupationFk = user.OccupationFk,
        Occupation = user.Occupation != null ? new OccupationDto
        {
          Id = user.Occupation.Id,
          Name = user.Occupation.Name
        } : null
      };
      userDtos.Add(userDto);
    }
    if (userDtos == null || userDtos.Count == 0)
    {
      throw new BadHttpRequestException("No se pudo obtener los usuarios.");
    }
    return userDtos;
  }

  public async Task<UserDto?> GetAsync(string id)
  {
    var user = await _userManager.Users
    .Include(u => u.Occupation)
    .AsNoTracking()
    .FirstOrDefaultAsync(u => u.Id == id);

    if (user == null)
    {
      throw new KeyNotFoundException("No se encontraron Usuarios.");
    }
    var role = await _userManager.GetRolesAsync(user);
    var userDto = new UserDto
    {
      Id = user.Id,
      FirstName = user.FirstName,
      LastName = user.LastName,
      Email = user.Email,
      DateOfBirth = user.DateOfBirth,
      ScoutUniqueId = user.ScoutUniqueId,
      Active = user.Active,
      Role = role.FirstOrDefault(),
      OccupationFk = user.OccupationFk,
      Occupation = user.Occupation != null ? new OccupationDto
      {
        Id = user.Occupation.Id,
        Name = user.Occupation.Name
      } : null
    };
    if (userDto == null)
    {
      throw new BadHttpRequestException("No se pudo obtener el usuario.");
    }
    return userDto;
  }

  public async Task<UserDto?> GetByEmailAsync(string email)
  {
    var user = await _userManager.Users
    .Include(u => u.Occupation)
    .AsNoTracking()
    .FirstOrDefaultAsync(u => u.Email == email);

    if (user == null)
    {
      throw new KeyNotFoundException("No se encontraron Usuarios.");
    }
    var role = await _userManager.GetRolesAsync(user);
    var userDto = new UserDto
    {
      Id = user.Id,
      FirstName = user.FirstName,
      LastName = user.LastName,
      Email = user.Email,
      DateOfBirth = user.DateOfBirth,
      ScoutUniqueId = user.ScoutUniqueId,
      Active = user.Active,
      Role = role.FirstOrDefault(),
      OccupationFk = user.OccupationFk,
      Occupation = user.Occupation != null ? new OccupationDto
      {
        Id = user.Occupation.Id,
        Name = user.Occupation.Name
      } : null
    };
    if (userDto == null)
    {
      throw new BadHttpRequestException("No se pudo obtener el usuario.");
    }
    return userDto;
  }
  public async Task<UserDto> UpdateAsync(UserDto userDto)
  {
    var user = await _userManager.FindByIdAsync(userDto.Id);
    if (user == null)
    {
      throw new KeyNotFoundException("No se encontraron Usuarios.");
    }
    user.FirstName = userDto.FirstName;
    user.LastName = userDto.LastName;
    user.Email = userDto.Email;
    user.DateOfBirth = userDto.DateOfBirth;
    user.ScoutUniqueId = userDto.ScoutUniqueId;
    user.Active = userDto.Active;
    user.OccupationFk = userDto.OccupationFk;
    var resultRole = await AssignRoleAsync(user, userDto.Role ?? "");
    if (!resultRole)
    {
      throw new BadHttpRequestException("Error al actualizar el usuario, no se pudo asignar el rol.");
    }
    await _userManager.UpdateAsync(user);
    return userDto;
  }

  public async Task<bool> DeleteAsync(string id)
  {
    var user = await _userManager.FindByIdAsync(id);
    if (user == null)
    {
      throw new KeyNotFoundException("No se encontraron Usuarios.");
    }
    user.Active = false;
    var result = await _userManager.UpdateAsync(user);
    return result.Succeeded;
  }

}
