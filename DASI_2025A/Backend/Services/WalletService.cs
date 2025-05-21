using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shared;

namespace Backend;

public class WalletService : IWalletService
{
	private readonly IWalletRepository _walletRepository;
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly IOrderRepository _orderRepository;

	public WalletService(
		IWalletRepository walletRepository,
		UserManager<ApplicationUser> userManager,
		 IOrderRepository orderRepository,
		 ApplicationDbContext context)
	{
		_walletRepository = walletRepository;
		_userManager = userManager;
		_orderRepository = orderRepository;
	}

	/// <summary>
	/// Obtiene el saldo actual del usuario
	/// </summary>
	public async Task<decimal> GetBalanceAsync(string userId)
	{
		return await _walletRepository.GetUserBalanceAsync(userId);
	}

	/// <summary>
	/// Obtiene el historial de transacciones del usuario
	/// </summary>
	public async Task<IEnumerable<WalletDto>> GetTransactionHistoryAsync(string userId)
	{
		return await _walletRepository.GetByUserIdAsync(userId);
	}

	/// <summary>
	/// Incrementa el saldo del usuario (top-up) usando métodos específicos
	/// </summary>
	public async Task<WalletDto> TopUpBalanceAsync(decimal amount, string userId, string description = "Recarga de saldo")
	{
		// Validar el usuario
		var user = await _userManager.FindByIdAsync(userId);
		if (user == null)
			throw new KeyNotFoundException("Usuario no encontrado");

		// Validar el monto
		if (amount <= 0)
			throw new ArgumentException("El monto debe ser mayor que cero", nameof(amount));

		try
		{
			// 1. Primero crear la orden usando el método específico
			var order = await _orderRepository.CreateTopUpOrderAsync(amount, userId, description ?? "Recarga de Saldo");

			// 2. Luego crear la transacción en wallet usando el método específico
			return await _walletRepository.CreateTopUpTransactionAsync(order.Id, amount, userId);
		}
		catch (Exception ex)
		{
			// Capturar y relanzar para mantener el mensaje original
			throw new Exception($"Error al realizar la recarga: {ex.Message}", ex);
		}
	}

	/// <summary>
	/// Obtiene una transacción específica por su ID
	/// </summary>
	public async Task<WalletDto?> GetTransactionAsync(int id)
	{
		return await _walletRepository.GetAsync(id);
	}
}