using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shared;

namespace Backend;

public class OrderService : IOrderService
{
	private readonly IOrderRepository _orderRepository;
	private readonly IProductRepository _productRepository;
	private readonly IWalletRepository _walletRepository;
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly IProductLoggerRepository _productLogger;

	public OrderService(
		IOrderRepository orderRepository,
		IProductRepository productRepository,
		IWalletRepository walletRepository,
		UserManager<ApplicationUser> userManager,
		IProductLoggerRepository productLogger)
	{
		_orderRepository = orderRepository;
		_productRepository = productRepository;
		_walletRepository = walletRepository;
		_userManager = userManager;
		_productLogger = productLogger;
	}

	/// <summary>
	/// Crea una nueva orden en el sistema
	/// </summary>
	public async Task<OrderDto> CreateOrderAsync(OrderDto orderDto, string userId)
	{
		// Validar el usuario
		var user = await _userManager.FindByIdAsync(userId);
		if (user == null || (!await _userManager.IsInRoleAsync(user, "Admin") && !await _userManager.IsInRoleAsync(user, "SuperAdmin")))
			throw new UnauthorizedAccessException("Solo un Administrador o Super Administrador puede realizar pedidos.");

		// Verificar que la orden tenga detalles
		if (orderDto.Details == null || !orderDto.Details.Any())
			throw new ArgumentException("La orden debe tener al menos un producto.");

		// Calcular el monto total de la orden
		orderDto.TotalAmount = orderDto.Details.Sum(d => d.UnitPrice * d.Quantity);

		// Asignar valores a la orden
		orderDto.OrderNote = $"Orden-{Guid.NewGuid()}";
		orderDto.OrderDate = DateTime.Now;
		orderDto.Status = "Confimada";
		orderDto.UserId = userId;

		// Verificar stock de todos los productos antes de proceder
		foreach (var detail in orderDto.Details)
		{
			var productDto = await _productRepository.GetAsync(detail.ProductId);
			if (productDto == null)
				throw new KeyNotFoundException($"El producto con ID {detail.ProductId} no existe.");

			if (productDto.Stock < detail.Quantity)
				throw new InvalidOperationException($"Stock insuficiente para {productDto.Name}. Disponible: {productDto.Stock}, Solicitado: {detail.Quantity}");
		}

		// Crear la orden
		var createdOrder = await _orderRepository.CreateAsync(orderDto, userId);

		// Registrar transacción en wallet
		var walletDto = new WalletDto
		{
			Action = "Discount",
			Status = "Completed",
			Amount = -orderDto.TotalAmount,
			OrderId = createdOrder.Id
		};

		await _walletRepository.CreateAsync(walletDto);

		return createdOrder;
	}
	/// <summary>
	/// Obtiene una orden por su ID
	/// </summary>
	public async Task<OrderDto?> GetOrderAsync(int orderId)
	{
		return await _orderRepository.GetAsync(orderId);
	}

	/// <summary>
	/// Obtiene todas las órdenes del sistema
	/// </summary>
	public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
	{
		return await _orderRepository.GetAllAsync();
	}

	/// <summary>
	/// Obtiene las órdenes de un usuario específico
	/// </summary>
	public async Task<IEnumerable<OrderDto>> GetUserOrdersAsync(string userId)
	{
		return await _orderRepository.GetByUserIdAsync(userId);
	}

	/// <summary>
	/// Obtiene el balance actual de un usuario
	/// </summary>
	public async Task<decimal> GetUserBalanceAsync(string userId)
	{
		return await _walletRepository.GetUserBalanceAsync(userId);
	}
}