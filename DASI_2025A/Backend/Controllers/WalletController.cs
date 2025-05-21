using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Backend;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class WalletController : ControllerBase
{
	private readonly IWalletService _walletService;

	public WalletController(IWalletService walletService)
	{
		_walletService = walletService;
	}

	/// <summary>
	/// Obtiene el saldo actual del usuario autenticado
	/// </summary>
	[HttpGet("balance")]
    [Authorize(Policy = "AdminPlus")]
	public async Task<ActionResult<decimal>> GetBalance()
	{
		try
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId))
				return Unauthorized(new { message = "Usuario no autenticado" });

			var balance = await _walletService.GetBalanceAsync(userId);
			return Ok(new { balance });
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = $"Error al obtener el saldo: {ex.Message}" });
		}
	}

	/// <summary>
	/// Obtiene el historial de transacciones del usuario autenticado
	/// </summary>
	[HttpGet("transactions")]
	[Authorize(Policy = "AdminPlus")]
	public async Task<ActionResult<IEnumerable<WalletDto>>> GetTransactionHistory()
	{
		try
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId))
				return Unauthorized(new { message = "Usuario no autenticado" });

			var transactions = await _walletService.GetTransactionHistoryAsync(userId);
			return Ok(transactions);
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = $"Error al obtener el historial de transacciones: {ex.Message}" });
		}
	}

	/// <summary>
	/// Incrementa el saldo del usuario autenticado (top-up)
	/// </summary>
	[HttpPost("topup")]
	[Authorize(Policy = "AdminPlus")]
	public async Task<ActionResult<WalletDto>> TopUp([FromBody] TopUpDto topUpDto)
	{
		try
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId))
				return Unauthorized(new { message = "Usuario no autenticado" });

			var transaction = await _walletService.TopUpBalanceAsync(topUpDto.Amount, userId, topUpDto.Description);

			// Obtener el saldo actualizado
			var updatedBalance = await _walletService.GetBalanceAsync(userId);

			return Ok(new
			{
				transaction,
				updatedBalance
			});
		}
		catch (ArgumentException ex)
		{
			return BadRequest(new { message = ex.Message });
		}
		catch (KeyNotFoundException ex)
		{
			return NotFound(new { message = ex.Message });
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = $"Error al realizar la recarga: {ex.Message}" });
		}
	}

	/// <summary>
	/// Obtiene los detalles de una transacción específica
	/// </summary>
	[HttpGet("transactions/{id}")]
	[Authorize(Policy = "AdminPlus")]
	public async Task<ActionResult<WalletDto>> GetTransaction(int id)
	{
		try
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId))
				return Unauthorized(new { message = "Usuario no autenticado" });

			var transaction = await _walletService.GetTransactionAsync(id);
			if (transaction == null)
				return NotFound(new { message = $"Transacción con ID {id} no encontrada" });

			// Verificar que la transacción pertenece al usuario o que el usuario es admin
			if (!User.IsInRole("Admin") && !User.IsInRole("SuperAdmin"))
			{
				// Necesitamos obtener todas las transacciones del usuario para verificar
				var userTransactions = await _walletService.GetTransactionHistoryAsync(userId);
				if (!userTransactions.Any(t => t.Id == id))
					return Unauthorized(new { message = "No tienes permiso para ver esta transacción" });
			}

			return Ok(transaction);
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = $"Error al obtener la transacción: {ex.Message}" });
		}
	}
}