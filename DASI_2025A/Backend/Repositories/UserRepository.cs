using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Backend;

public class UserRepository : IUserRepository
{
  private readonly UserManager<ApplicationUser> _userManager;
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
    ApplicationDbContext context
    )
  {
    _userManager = userManager;
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
    if (string.IsNullOrEmpty(roleName))
    {
      throw new ArgumentException("El rol es requerido.");
    }
    var roles = await _userManager.GetRolesAsync(user);
    // Elimina todos los roles actuales
    if (roles.Any())
    {
      var removeResult = await _userManager.RemoveFromRolesAsync(user, roles);
      if (!removeResult.Succeeded)
      {
        return false;
      }
    }
    // Asigna el nuevo rol
    var addResult = await _userManager.AddToRoleAsync(user, roleName);
    return addResult.Succeeded;
  }

  /// <summary>
  /// Crea un nuevo usuario en la base de datos.
  /// </summary>
  /// <param name="userDto">Objeto con los datos del usuario a crear.</param>
  /// <returns>Objeto con los datos del usuario recién creado.</returns>
  /// <exception cref="ArgumentException">Si el password es nulo o vacío.</exception>
  /// <exception cref="BadHttpRequestException">Si no se pudo crear el usuario.</exception>
  public async Task<(UserDto userDto, string token)> CreateAsync(UserDto userDto)
  {
    var user = new ApplicationUser
    {
      Email = userDto.Email,
      FirstName = userDto.FirstName,
      LastName = userDto.LastName,
      DateOfBirth = userDto.DateOfBirth,
      ScoutUniqueId = userDto.ScoutUniqueId,
      Active = false,
      OccupationFk = userDto.OccupationFk,
      BranchFk = userDto.BranchFk
    };
    string password = GenerateValidPassword();
    var result = await _userManager.CreateAsync(user, password);
    if (!result.Succeeded)
    {
      var errors = string.Join(" | ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
      throw new BadHttpRequestException($"Error al crear usuario: {errors}");
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
    BranchEntity? branch = user.Branch;
    userDto.Branch = branch != null ? new BranchDto
    {
      Id = branch.Id,
      Name = branch.Name
    } : null;
    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
    return (userDto, token);
  }

  /// <summary>
  /// Activa un usuario, cambia la contraseña por una establecida por el usuario, acepta políticas de privacidad y términos y condiciones(EmailConfirmed).
  /// </summary>
  /// <param name="activateUserDto">Objeto que contiene el correo electrónico del usuario, el token de activación y la nueva contraseña.</param>
  /// <returns>
  ///     Retorna un <see cref="bool"/> indicando si la operación de activación del usuario fue exitosa.
  ///     Lanza una excepción <see cref="KeyNotFoundException"/> si no se encontró el usuario.
  ///     Lanza una excepción <see cref="BadHttpRequestException"/> si el usuario ya esta activo, o hubo un error al resetear la contraseña o al actualizar el usuario.
  /// </returns>
  public async Task<bool> ActivateUserAsync(ActivateUserDto activateUserDto)
  {
    var user = await _userManager.FindByEmailAsync(activateUserDto.Email);
    if (user == null) throw new KeyNotFoundException("No se encontró este Usuario.");
    user.Active = true;
    user.EmailConfirmed = true;
    var resetPass = await _userManager.ResetPasswordAsync(user, activateUserDto.Token, activateUserDto.NewPassword!);
    foreach (var error in resetPass.Errors)
    {
      if (error.Description.Contains("invalid")) throw new UnauthorizedAccessException("Solicitud invalida o expirada.");
    }
    if (!resetPass.Succeeded) throw new BadHttpRequestException("Error al activar el usuario 1.");
    var result = await _userManager.UpdateAsync(user);
    if (!result.Succeeded) throw new BadHttpRequestException("Error al activar el usuario 2.");
    return true;
  }

  /// <summary>
  /// Obtiene todos los usuarios de la base de datos.
  /// </summary>
  /// <returns>
  ///     Retorna un <see cref="IList{UserDto}"/> con todos los usuarios de la base de datos.
  ///     Si no se encontraron usuarios, lanza una excepción <see cref="KeyNotFoundException"/>.
  ///     Si ocurre un error inesperado, lanza una excepción <see cref="BadHttpRequestException"/>.
  /// </returns>
  public async Task<PagedResult<UserDto>> GetAllAsync(UserQueryParams queryParams)
  {
    var query = _userManager.Users
      .Include(u => u.Occupation)
      .Include(u => u.Branch)
      .Where(u => u.Active == true)
      .AsNoTracking()
      .AsQueryable();

    // Filtro por nombre/apellido
    if (!string.IsNullOrWhiteSpace(queryParams.SearchName))
    {
      var search = queryParams.SearchName.ToLower();
      query = query.Where(u =>
        u.FirstName.ToLower().Contains(search) ||
        u.LastName.ToLower().Contains(search));
    }

    if (!string.IsNullOrWhiteSpace(queryParams.SearchEmail))
    {
      Console.WriteLine(queryParams.SearchEmail);
      var search = queryParams.SearchEmail.ToLower();
      query = query.Where(u =>
        u.Email!.ToLower().Contains(search)
      );
    }

    // Filtro por ocupación
    if (queryParams.OccupationFk.HasValue)
    {
      query = query.Where(u => u.OccupationFk == queryParams.OccupationFk.Value);
    }

    // Filtro por branch
    if (queryParams.BranchFk.HasValue)
    {
      query = query.Where(u => u.BranchFk == queryParams.BranchFk.Value);
    }
    if (!string.IsNullOrWhiteSpace(queryParams.Role))
    {
      var roleToMatch = queryParams.Role;
      var userIdsWithRole = (await _userManager.GetUsersInRoleAsync(roleToMatch)).Select(u => u.Id);
      query = query.Where(u => userIdsWithRole.Contains(u.Id));
    }

    // Total sin paginación
    var totalItems = await query.CountAsync();

    // Aplicar paginación
    var users = await query
      .OrderByDescending(u => u.Id)
      .Skip(queryParams.Skip)
      .Take(queryParams.PageSize)
      .ToListAsync();

    if (users == null || users.Count == 0)
    {
      return new PagedResult<UserDto>
      {
        Items = [],
        TotalItems = 0
      };
    }

    // Obtener todos los roles
    var userRoles = new Dictionary<string, IList<string>>();
    foreach (var user in users)
    {
      userRoles[user.Id] = await _userManager.GetRolesAsync(user);
    }

    // Mapear a DTO
    var userDtos = users.Select(user =>
    {
      var roles = userRoles[user.Id];
      return new UserDto
      {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Email = user.Email ?? string.Empty,
        DateOfBirth = user.DateOfBirth,
        ScoutUniqueId = user.ScoutUniqueId,
        Active = user.Active,
        Role = roles.FirstOrDefault(),
        OccupationFk = user.OccupationFk,
        Balance = user.Balance,
        Occupation = user.Occupation != null ? new OccupationDto
        {
          Id = user.Occupation.Id,
          Name = user.Occupation.Name
        } : null,
        BranchFk = user.BranchFk,
        Branch = user.Branch != null ? new BranchDto
        {
          Id = user.Branch.Id,
          Name = user.Branch.Name
        } : null
      };
    }).ToList();

    return new PagedResult<UserDto>
    {
      Items = userDtos,
      TotalItems = totalItems
    };
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
    .Include(u => u.Branch)
    .AsNoTracking()
    .FirstOrDefaultAsync(u => u.Id == id);

    if (user == null || user.Active == false)
    {
      throw new KeyNotFoundException("No se encontraron Usuarios.");
    }
    var role = await _userManager.GetRolesAsync(user);
    var userDto = new UserDto
    {
      Id = user.Id,
      FirstName = user.FirstName,
      LastName = user.LastName,
      Email = user.Email ?? string.Empty,
      DateOfBirth = user.DateOfBirth,
      ScoutUniqueId = user.ScoutUniqueId,
      Active = user.Active,
      Balance = user.Balance,
      Role = role.FirstOrDefault(),
      OccupationFk = user.OccupationFk,
      Occupation = user.Occupation != null ? new OccupationDto
      {
        Id = user.Occupation.Id,
        Name = user.Occupation.Name
      } : null,
      BranchFk = user.BranchFk,
      Branch = user.Branch != null ? new BranchDto
      {
        Id = user.Branch.Id,
        Name = user.Branch.Name
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
    .Include(u => u.Branch)
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
      Email = user.Email ?? string.Empty,
      DateOfBirth = user.DateOfBirth,
      ScoutUniqueId = user.ScoutUniqueId,
      Active = user.Active,
      Role = role.FirstOrDefault(),
      OccupationFk = user.OccupationFk,
      Occupation = user.Occupation != null ? new OccupationDto
      {
        Id = user.Occupation.Id,
        Name = user.Occupation.Name
      } : null,
      BranchFk = user.BranchFk,
      Branch = user.Branch != null ? new BranchDto
      {
        Id = user.Branch.Id,
        Name = user.Branch.Name
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
    user.BranchFk = userDto.BranchFk;
    var resultRole = await AssignRoleAsync(user, string.IsNullOrWhiteSpace(userDto.Role) ? "User" : userDto.Role);
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

  /// <summary>
  ///     Cambia la contraseña de un usuario.
  /// </summary>
  /// <param name="userId">El identificador del usuario.</param>
  /// <param name="changePassDto">Objeto con los datos de la contraseña actual y la nueva.</param>
  /// <returns>
  ///     Retorna un <see cref="bool"/> con el resultado de la operación.
  ///     Lanza una excepción <see cref="BadHttpRequestException"/> si el usuario no esta activo o la contraseña antigua es incorrecta, o
  ///     si las contraseñas no coinciden.
  /// </returns>
  public async Task<bool> ChangePasswordAsync(string userId, ChangePassDto changePassDto)
  {
    var user = await _userManager.FindByIdAsync(userId);
    if (user == null)
    {
      throw new BadHttpRequestException("Usuario no registrado.");
    }
    if (!user.Active)
    {
      throw new BadHttpRequestException("El usuario no esta activo.");
    }
    var checkPassword = await _userManager.CheckPasswordAsync(user, changePassDto.OldPassword!);
    if (!checkPassword)
    {
      throw new BadHttpRequestException("La contraseña antigua es incorrecta.");
    }
    if (changePassDto.NewPassword != changePassDto.ConfirmNewPassword)
    {
      throw new BadHttpRequestException("Las contraseñas no coinciden.");
    }
    var result = await _userManager.ChangePasswordAsync(user, changePassDto.OldPassword!, changePassDto.NewPassword!);
    return result.Succeeded;
  }

  /// <summary>
  ///     Genera un token de recuperación de contraseña para un usuario registrado.
  /// </summary>
  /// <param name="email">El correo electrónico del usuario.</param>
  /// <returns>
  ///     Retorna una tupla con el token de recuperación de contraseña y el primer nombre del usuario.
  ///     Lanza una excepción <see cref="KeyNotFoundException"/> si no se encuentra el usuario.
  ///     Lanza una excepción <see cref="BadHttpRequestException"/> si el usuario no está activo.
  /// </returns>
  public async Task<(string Token, string FirstName)> RecoverPasswordAsync(string email)
  {
    var user = await _userManager.FindByEmailAsync(email);
    if (user == null)
    {
      throw new KeyNotFoundException("Usuario no registrado.");
    }
    if (!user.Active)
    {
      throw new BadHttpRequestException("El usuario no esta activo.");
    }
    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
    return (token, user.FirstName);
  }

  /// <summary>
  ///     Resetea la contraseña de un usuario al que se le ha enviado un correo de recuperación.
  /// </summary>
  /// <param name="recoverPassDto">Objeto que contiene el correo electrónico del usuario, el token de recuperación y la nueva contraseña.</param>
  /// <returns>
  ///     Retorna un <see cref="bool"/> indicando si la operación de reseteo de contraseña fue exitosa.
  ///     Lanza una excepción <see cref="KeyNotFoundException"/> si no se encuentra el usuario.
  ///     Lanza una excepción <see cref="BadHttpRequestException"/> si el usuario no está activo.
  /// </returns>
  public async Task<bool> RecoverPasswordAsync(RecoverPassDto recoverPassDto)
  {
    var user = await _userManager.FindByEmailAsync(recoverPassDto.Email);
    if (user == null)
    {
      throw new KeyNotFoundException("Usuario no registrado.");
    }
    if (!user.Active)
    {
      throw new BadHttpRequestException("El usuario no esta activo.");
    }
    var result = await _userManager.ResetPasswordAsync(user, recoverPassDto.Token, recoverPassDto.NewPassword!);
    return result.Succeeded;
  }

  /// <summary>
  ///     Obtiene las transacciones de un usuario.
  /// </summary>
  /// <param name="userId">El identificador del usuario.</param>
  /// <returns>
  ///     Retorna un <see cref="UserTransactionsDto"/> que contiene las transacciones del usuario.
  ///     Lanza una excepción <see cref="KeyNotFoundException"/> si no se encontraron usuarios.
  /// </returns>
  public async Task<UserTransactionsDto> GetUserTransactionsAsync(string userId)
  {
    var user = await _userManager.FindByIdAsync(userId);
    if (user == null)
    {
      throw new KeyNotFoundException("No se encontraron Usuarios.");
    }
    var transactions = await _context.BalanceTransactions.
    Where(t => t.UserId == userId).OrderByDescending(t => t.Id).ToListAsync();
    UserTransactionsDto userTransactionsDto = new UserTransactionsDto
    {
      UserId = user.Id,
      FirstName = user.FirstName,
      LastName = user.LastName,
      Balance = user.Balance,
      Transactions = transactions.Select(t => new BalanceTransactionDto
      {
        Id = t.Id,
        Amount = t.Amount,
        Type = t.Type,
        BalanceAfter = t.BalanceAfter,
        Description = t.Description,
        AuditableDate = t.AuditableDate
      }).ToList()
    };
    return userTransactionsDto;
  }

  /// <summary>
  ///     Crea una solicitud de recarga para un usuario.
  /// </summary>
  /// <param name="topUpRequestDto">Objeto con los datos de la solicitud de recarga.</param>
  /// <returns>
  ///     Retorna un <see cref="TopUpRequestResponseDto"/> que contiene los datos de la solicitud de recarga recién creada.
  ///     Lanza una excepción <see cref="KeyNotFoundException"/> si el usuario no existe o no esta activo.
  ///     Lanza una excepción <see cref="Exception"/> si no se pudo insertar la solicitud de recarga.
  /// </returns>
  public async Task<TopUpRequestResponseDto> CreateTopUpRequestAsync(TopUpRequestCreateDto topUpRequestDto)
  {
    var user = _userManager.Users.FirstOrDefault(u => u.Id == topUpRequestDto.TargetUserId);
    if (user == null || !user.Active)
    {
      throw new KeyNotFoundException("No se encontró el usuario destinatario.");
    }
    var TopUpRequest = new TopUpRequestEntity
    {
      Amount = topUpRequestDto.Amount,
      Type = topUpRequestDto.Type,
      TargetUserId = topUpRequestDto.TargetUserId,
      RequestedByUserId = topUpRequestDto.RequestedByUserId
    };

    if (topUpRequestDto.Receipt != null)
    {
      var imageData = Convert.FromBase64String(topUpRequestDto.Receipt);
      var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Comprobantes");
      if (!Directory.Exists(folderPath))
      {
        Directory.CreateDirectory(folderPath);
      }
      var fileName = $"{Guid.NewGuid()}.png";
      var filePath = Path.Combine(folderPath, fileName);
      using (var stream = new FileStream(filePath, FileMode.Create))
      {
        await stream.WriteAsync(imageData, 0, imageData.Length);
      }
      TopUpRequest.Receipt = filePath;
    }
    _context.TopUpRequests.Add(TopUpRequest);
    var result = await _context.SaveChangesAsync();
    if (result == 0)
    {
      throw new Exception("No se pudo insertar la solicitud de recarga.");
    }
    var insertedRequest = await _context.TopUpRequests
    .Include(r => r.RequestedByUser)
    .Include(r => r.TargetUser).FirstOrDefaultAsync(r => r.Id == TopUpRequest.Id);

    return new TopUpRequestResponseDto
    {
      Id = insertedRequest!.Id,
      Amount = insertedRequest.Amount,
      Type = insertedRequest.Type,
      TargetUser = insertedRequest.TargetUser!.FirstName + " " + insertedRequest.TargetUser.LastName,
      RequestedByUser = insertedRequest.RequestedByUser!.FirstName + " " + insertedRequest.RequestedByUser.LastName,
      Receipt = insertedRequest.Receipt
    };
  }

  /// <summary>
  ///   Aprobar o rechazar una solicitud de recarga de saldo.
  /// </summary>
  /// <param name="topUpRequestDto">El objeto con los datos de la solicitud de recarga.</param>
  /// <returns>El objeto con los datos de la solicitud de recarga actualizada.</returns>
  /// <exception cref="KeyNotFoundException">Si no se encuentra la solicitud de recarga.</exception>
  /// <exception cref="InvalidOperationException">Si la solicitud de recarga no tiene estado PENDIENTE.</exception>
  /// <exception cref="BadHttpRequestException">Si no se puede actualizar el saldo del usuario.</exception>
  public async Task<TopUpRequestResponseDto> AproveOrRejectTopUpAsync(TopUpRequestUpdateDto topUpRequestDto)
  {
    var topUpRequest = await _context.TopUpRequests
        .Include(r => r.RequestedByUser)
        .Include(r => r.TargetUser)
        .Include(r => r.AuthorizedByUser)
        .FirstOrDefaultAsync(r => r.Id == topUpRequestDto.Id);

    if (topUpRequest == null)
    {
      throw new KeyNotFoundException("No se encontró la solicitud de recarga.");
    }

    if (!string.Equals(topUpRequest.Status, "PENDIENTE", StringComparison.OrdinalIgnoreCase))
    {
      throw new InvalidOperationException("Solo se pueden aprobar o rechazar solicitudes con estado PENDIENTE.");
    }

    topUpRequest.Status = topUpRequestDto.Status!;
    topUpRequest.AuthorizedByUserId = topUpRequestDto.AuthorizedByUserId;

    if (topUpRequestDto.Status == "APROBADO")
    {
      var user = await _userManager.FindByIdAsync(topUpRequest.TargetUserId);
      if (user == null)
      {
        throw new KeyNotFoundException("No se encontró el usuario destinatario.");
      }

      user.Balance += topUpRequest.Amount;
      var updateResult = await _userManager.UpdateAsync(user);
      if (!updateResult.Succeeded)
      {
        throw new BadHttpRequestException("No se pudo actualizar el saldo del usuario.");
      }
    }

    var result = await _context.SaveChangesAsync();

    return new TopUpRequestResponseDto
    {
      Id = topUpRequest.Id,
      Amount = topUpRequest.Amount,
      Type = topUpRequest.Type,
      TargetUser = topUpRequest.TargetUser != null ? $"{topUpRequest.TargetUser.FirstName} {topUpRequest.TargetUser.LastName}" : "Desconocido",
      RequestedByUser = topUpRequest.RequestedByUser != null ? $"{topUpRequest.RequestedByUser.FirstName} {topUpRequest.RequestedByUser.LastName}" : "Desconocido",
      Receipt = topUpRequest.Receipt,
      Status = topUpRequest.Status,
      AuthorizedByUser = topUpRequest.AuthorizedByUser != null ? $"{topUpRequest.AuthorizedByUser.FirstName} {topUpRequest.AuthorizedByUser.LastName}" : null,
    };
  }

  /// <summary>
  ///   Obtiene todas las solicitudes de recarga de saldo.
  /// </summary>
  /// <returns>Una lista de objetos con los datos de las solicitudes de recarga.</returns>
  /// <exception cref="KeyNotFoundException">Si no se encontraron solicitudes de recarga.</exception>
  public async Task<PagedResult<TopUpRequestResponseDto>> GetTopUpRequestsAsync(AdminTopUpRequestQueryParams query)
  {
    var baseQuery = _context.TopUpRequests
      .Include(r => r.RequestedByUser)
      .Include(r => r.TargetUser)
      .Include(r => r.AuthorizedByUser)
      .AsQueryable();

    // Filtros dinámicos
    if (!string.IsNullOrWhiteSpace(query.Type))
      baseQuery = baseQuery.Where(r => r.Type == query.Type);

    if (!string.IsNullOrWhiteSpace(query.Status))
      baseQuery = baseQuery.Where(r => r.Status == query.Status);

    if (!string.IsNullOrWhiteSpace(query.TargetUser))
      baseQuery = baseQuery.Where(r => (r.TargetUser!.FirstName + " " + r.TargetUser.LastName).Contains(query.TargetUser));

    if (!string.IsNullOrWhiteSpace(query.AuthorizedByUser))
      baseQuery = baseQuery.Where(r => r.AuthorizedByUser != null && (r.AuthorizedByUser.FirstName + " " + r.AuthorizedByUser.LastName).Contains(query.AuthorizedByUser));

    if (query.StartDate.HasValue)
      baseQuery = baseQuery.Where(r => r.AuditableDate >= query.StartDate.Value);

    if (query.EndDate.HasValue)
      baseQuery = baseQuery.Where(r => r.AuditableDate <= query.EndDate.Value);

    var totalCount = await baseQuery.CountAsync();

    var result = await baseQuery
      .OrderByDescending(r => r.Id)
      .Skip(query.Skip)
      .Take(query.PageSize)
      .ToListAsync();

    var mapped = result.Select(request => new TopUpRequestResponseDto
    {
      Id = request.Id,
      Amount = request.Amount,
      Type = request.Type,
      Status = request.Status,
      Receipt = request.Receipt != null
        ? $"data:image/png;base64,{Convert.ToBase64String(File.ReadAllBytes(request.Receipt))}"
        : null,
      TargetUser = request.TargetUser != null ? $"{request.TargetUser.FirstName} {request.TargetUser.LastName}" : "Desconocido",
      RequestedByUser = request.RequestedByUser != null ? $"{request.RequestedByUser.FirstName} {request.RequestedByUser.LastName}" : "Desconocido",
      AuthorizedByUser = request.AuthorizedByUser != null ? $"{request.AuthorizedByUser.FirstName} {request.AuthorizedByUser.LastName}" : null,
      AuditableDate = request.AuditableDate,
      MachineName = request.MachineName
    }).ToList();

    return new PagedResult<TopUpRequestResponseDto>
    {
      Items = mapped,
      TotalItems = totalCount
    };
  }


  /// <summary>
  ///   Obtiene las solicitudes de recarga de saldo del usuario con el identificador especificado.
  /// </summary>
  /// <param name="userId">El identificador del usuario.</param>
  /// <returns>Una lista de objetos con los datos de las solicitudes de recarga.</returns>
  /// <exception cref="KeyNotFoundException">Si no se encontró el usuario o no se encontraron solicitudes de recarga para el usuario.</exception>
  public async Task<IEnumerable<TopUpRequestResponseDto>> GetTopUpRequestsByUserIdAsync(string userId)
  {
    var user = await _userManager.FindByIdAsync(userId);
    if (user == null)
    {
      throw new KeyNotFoundException("No se encontró el usuario..");
    }
    var results = await _context.TopUpRequests
        .Include(r => r.RequestedByUser)
        .Include(r => r.TargetUser)
        .Include(r => r.AuthorizedByUser)
        .Where(r => r.TargetUserId == userId)
        .OrderByDescending(r => r.Id)
        .ToListAsync();

    if (results == null || results.Count == 0)
    {
      throw new KeyNotFoundException($"No se encontraron solicitudes de recarga para el usuario {user.FirstName + " " + user.LastName}.");
    }

    var requests = results.Select(request => new TopUpRequestResponseDto
    {
      Id = request.Id,
      Amount = request.Amount,
      Type = request.Type,
      TargetUser = $"{request.TargetUser?.FirstName ?? ""} {request.TargetUser?.LastName ?? ""}".Trim(),
      RequestedByUser = $"{request.RequestedByUser?.FirstName ?? ""} {request.RequestedByUser?.LastName ?? ""}".Trim(),
      Receipt = request.Receipt != null
        ? $"data:image/png;base64,{Convert.ToBase64String(File.ReadAllBytes(request.Receipt))}"
        : null,
      Status = request.Status,
      AuditableDate = request.AuditableDate,
      AuthorizedByUser = $"{request.AuthorizedByUser?.FirstName ?? ""} {request.AuthorizedByUser?.LastName ?? ""}".Trim()
    }).ToList();

    return requests;
  }

  private string GenerateValidPassword()
  {
    var random = new Random();
    const string mayus = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    const string minus = "abcdefghijklmnopqrstuvwxyz";
    const string numeros = "0123456789";
    const string especiales = "!@#$%^&*()_-+=<>?";

    // Asegurar al menos un carácter de cada tipo
    var chars = new List<char>
    {
        mayus[random.Next(mayus.Length)],
        minus[random.Next(minus.Length)],
        numeros[random.Next(numeros.Length)],
        especiales[random.Next(especiales.Length)]
    };

    // Rellenar hasta llegar a 8 caracteres
    string todos = mayus + minus + numeros + especiales;
    while (chars.Count < 8)
    {
      chars.Add(todos[random.Next(todos.Length)]);
    }

    // Mezclar los caracteres
    return new string(chars.OrderBy(_ => random.Next()).ToArray());
  }

}
