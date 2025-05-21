using Microsoft.EntityFrameworkCore;
using Shared;
using System.Security.Claims;

namespace Backend;

public class WalletRepository : IWalletRepository
{
	private readonly ApplicationDbContext _context;

	/// <summary>
	/// Inicializa una nueva instancia de <see cref="WalletRepository"/>.
	/// </summary>
	/// <param name="context">El contexto de la base de datos.</param>
	public WalletRepository(ApplicationDbContext context)
	{
		_context = context;
	}

	/// <summary>
	/// Crea una nueva transacción de wallet en la base de datos.
	/// </summary>
	/// <param name="walletDto">Datos de la transacción a crear.</param>
	/// <returns>La transacción creada con su ID asignado.</returns>
	public async Task<WalletDto> CreateAsync(WalletDto walletDto)
	{
		var entity = new WalletEntity
		{
			Action = walletDto.Action,
			Status = walletDto.Status,
			Amount = walletDto.Amount,
			OrderId = walletDto.OrderId,
			UserId = walletDto.UserId,
		};
		_context.Wallets.Add(entity);
		var result = await _context.SaveChangesAsync();
		if (result == 0)
		{
			throw new Exception("No se pudo registrar la transacción en la wallet.");
		}
		walletDto.Id = entity.Id;
		walletDto.AuditableDate = entity.AuditableDate;
		return walletDto;
	}

	/// <summary>
	/// Obtiene todas las transacciones de wallet por ID de orden.
	/// </summary>
	/// <param name="orderId">ID de la orden a buscar.</param>
	/// <returns>Lista de transacciones relacionadas con la orden.</returns>
	public async Task<IEnumerable<WalletDto>> GetByOrderIdAsync(int orderId)
	{
		return await _context.Wallets
			.Where(w => w.OrderId == orderId)
			.OrderByDescending(w => w.AuditableDate)
			.Select(w => new WalletDto
			{
				Id = w.Id,
				Action = w.Action,
				Status = w.Status,
				Amount = w.Amount,
				OrderId = w.OrderId,
				AuditableDate = w.AuditableDate
			})
			.ToListAsync();
	}

	/// <summary>
	/// Obtiene todas las transacciones de wallet relacionadas con un usuario específico.
	/// </summary>
	/// <param name="userId">ID del usuario.</param>
	/// <returns>Lista de transacciones del usuario.</returns>
	public async Task<IEnumerable<WalletDto>> GetByUserIdAsync(string userId)
	{
		return await _context.Wallets
			.Join(_context.Orders,
				wallet => wallet.OrderId,
				order => order.Id,
				(wallet, order) => new { Wallet = wallet, Order = order })
			.Where(join => join.Order.UserId == userId)
			.OrderByDescending(join => join.Wallet.AuditableDate)
			.Select(join => new WalletDto
			{
				Id = join.Wallet.Id,
				Action = join.Wallet.Action,
				Status = join.Wallet.Status,
				Amount = join.Wallet.Amount,
				OrderId = join.Wallet.OrderId,
				AuditableDate = join.Wallet.AuditableDate,
				UserId = join.Wallet.UserId
			})
			.ToListAsync();
	}

	/// <summary>
	/// Obtiene el balance actual de un usuario sumando todas sus transacciones.
	/// </summary>
	/// <param name="userId">ID del usuario.</param>
	/// <returns>Balance actual del usuario.</returns>
	public async Task<decimal> GetUserBalanceAsync(string userId)
	{
		var transactions = await GetByUserIdAsync(userId);
		return transactions.Sum(t => t.Amount);
	}

	/// <summary>
	/// Obtiene una transacción específica por su ID.
	/// </summary>
	/// <param name="id">ID de la transacción a buscar.</param>
	/// <returns>La transacción si existe, null en caso contrario.</returns>
	public async Task<WalletDto?> GetAsync(int id)
	{
		var entity = await _context.Wallets.FindAsync(id);

		if (entity == null)
		{
			return null;
		}

		return new WalletDto
		{
			Id = entity.Id,
			Action = entity.Action,
			Status = entity.Status,
			Amount = entity.Amount,
			OrderId = entity.OrderId,
			AuditableDate = entity.AuditableDate
		};
	}

	/// <summary>
	/// Registra una transacción de venta en la wallet
	/// </summary>
	public async Task<WalletDto> RegisterSaleTransactionAsync(int orderId, decimal amount, string userId)
	{
		// Validar que el monto sea positivo
		if (amount <= 0)
			throw new ArgumentException("El monto debe ser positivo", nameof(amount));
		// Verifica que el saldo total sea mayor a -10
		var totalAmount = await GetUserBalanceAsync(userId);
		if (totalAmount < -10)
			throw new InvalidOperationException("Saldo insuficiente en la wallet para realizar la venta.");
		// Crear la transacción con monto negativo (descuento del balance)
		var walletDto = new WalletDto
		{
			Action = "Descuento",
			Status = "Completado",
			Amount = -amount, // Convertir a negativo para indicar un cargo
			OrderId = orderId,
			UserId = userId
		};
		return await CreateAsync(walletDto);
	}
	/// <summary>
	/// Crea una transacción específica para recargas de saldo
	/// </summary>
	public async Task<WalletDto> CreateTopUpTransactionAsync(int orderId, decimal amount, string userId)
	{
		// Validar el monto
		if (amount <= 0)
			throw new ArgumentException("El monto debe ser positivo", nameof(amount));

		// Crear la entidad de transacción de wallet
		var walletEntity = new WalletEntity
		{
			Action = "Recarga",
			Status = "Completado",
			Amount = amount, // Positivo para indicar ingreso
			OrderId = orderId,
			UserId = userId
		};

		// Agregar al contexto
		_context.Wallets.Add(walletEntity);
		await _context.SaveChangesAsync();

		// Mapear a DTO y devolver
		return new WalletDto
		{
			Id = walletEntity.Id,
			Action = walletEntity.Action,
			Status = walletEntity.Status,
			Amount = walletEntity.Amount,
			OrderId = walletEntity.OrderId,
			AuditableDate = walletEntity.AuditableDate,
			UserId = walletEntity.UserId
		};
	}
}