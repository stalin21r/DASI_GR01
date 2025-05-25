using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared;


namespace Backend;
public class UserRepository : IUserRepository
{
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly RoleManager<IdentityRole> _roleManager;
  private readonly ApplicationDbContext _context;

  /// <summary>
  /// Constructor de la clase UserRepository.
  /// Inicializa una nueva instancia de la clase UserRepository con los servicios necesarios para la gestión de usuarios y roles.
  /// </summary>
  /// <param name="userManager">UserManager para gestionar usuarios de la aplicación.</param>
  /// <param name="roleManager">RoleManager para gestionar roles de la aplicación.</param>
  /// <param name="context">Contexto de la base de datos de la aplicación.</param>

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
  /// <summary>
  /// Asigna un rol a un usuario.
  /// </summary>
  /// <param name="user">Usuario al que se le va a asignar el rol.</param>
  /// <param name="roleName">Nombre del rol a asignar.</param>
  /// <returns>True si se asignó el rol, false en caso de error.</returns>
  /// <exception cref="ArgumentException">Si el rol es nulo o vacío.</exception>
  /// <exception cref="BadHttpRequestException">Si no se pudo asignar el rol.</exception>
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


  /// <summary>
  /// Crea un nuevo usuario en la base de datos.
  /// </summary>
  /// <param name="userDto">Objeto con los datos del usuario a crear.</param>
  /// <returns>Objeto con los datos del usuario recién creado.</returns>
  /// <exception cref="ArgumentException">Si el password es nulo o vacío.</exception>
  /// <exception cref="BadHttpRequestException">Si no se pudo crear el usuario.</exception>
  public async Task<UserDto> CreateAsync(UserDto userDto)
  {
    var user = new ApplicationUser
    {
      Email = userDto.Email,
      FirstName = userDto.FirstName,
      LastName = userDto.LastName,
      DateOfBirth = userDto.DateOfBirth,
      ScoutUniqueId = userDto.ScoutUniqueId,
      AccountBalance = userDto.AccountBalance,
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
      throw new BadHttpRequestException("Error al crear usuario. Puede que el usuario ya este registrado.");
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

  /// <summary>
  /// Obtiene todos los usuarios de la base de datos.
  /// </summary>
  /// <returns>
  ///     Retorna un <see cref="IList{UserDto}"/> con todos los usuarios de la base de datos.
  ///     Si no se encontraron usuarios, lanza una excepción <see cref="KeyNotFoundException"/>.
  ///     Si ocurre un error inesperado, lanza una excepción <see cref="BadHttpRequestException"/>.
  /// </returns>
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

  /// <summary>
  /// Obtiene un usuario por su ID.
  /// </summary>
  /// <param name="id">El identificador del usuario.</param>
  /// <returns>
  ///     Retorna un <see cref="UserDto"/> con los datos del usuario, si se encuentra.
  ///     Retorna <see langword="null"/> si no se encontró el usuario.
  ///     Lanza una excepción <see cref="KeyNotFoundException"/> si no se encontraron usuarios.
  ///     Lanza una excepción <see cref="BadHttpRequestException"/> si ocurre un error inesperado.
  /// </returns>
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

  /// <summary>
  ///     Obtiene un usuario por su correo electrónico.
  /// </summary>
  /// <param name="email">El correo electrónico del usuario.</param>
  /// <returns>
  ///     Retorna un objeto <see cref="UserDto"/> que representa el usuario.
  ///     Retorna <see langword="null"/> si no se encontró el usuario.
  ///     Lanza una excepción <see cref="KeyNotFoundException"/> si no se encontraron usuarios.
  ///     Lanza una excepción <see cref="BadHttpRequestException"/> si ocurre un error inesperado.
  /// </returns>
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
  /// <summary>
  ///     Actualiza un usuario.
  /// </summary>
  /// <param name="userDto">Objeto con los datos del usuario a actualizar.</param>
  /// <returns>
  ///     Retorna el objeto <see cref="UserDto"/> actualizado.
  ///     Lanza una excepción <see cref="KeyNotFoundException"/> si no se encontró el usuario.
  ///     Lanza una excepción <see cref="BadHttpRequestException"/> si ocurre un error inesperado.
  /// </returns>
  public async Task<UserDto> UpdateAsync(UserDto userDto)
  {
    var user = await _userManager.FindByIdAsync(userDto.Id!);
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

  /// <summary>
  ///     Elimina un usuario por su ID. En realidad, lo que hace es desactivar la cuenta del usuario.
  /// </summary>
  /// <param name="id">El identificador del usuario a eliminar.</param>
  /// <returns>
  ///     Retorna un <see cref="bool"/> con el resultado de la operación.
  ///     Lanza una excepción <see cref="KeyNotFoundException"/> si no se encontró el usuario.
  /// </returns>
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
