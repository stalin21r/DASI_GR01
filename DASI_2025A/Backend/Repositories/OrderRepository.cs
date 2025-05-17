using Microsoft.EntityFrameworkCore;
using Shared;

namespace Backend;

public class OrderRepository : IOrderRepository
{
	private readonly ApplicationDbContext _context;
	private readonly IProductRepository _productRepository;

	/// <summary>
	/// Inicializa una nueva instancia de <see cref="OrderRepository"/>.
	/// </summary>
	/// <param name="context">El contexto de la base de datos.</param>
	/// <param name="productRepository">Repositorio de productos.</param>
	public OrderRepository(ApplicationDbContext context, IProductRepository productRepository)
	{
		_context = context;
		_productRepository = productRepository;
	}

	/// <summary>
	/// Crea una nueva orden en la base de datos.
	/// </summary>
	/// <param name="orderDto">Datos de la orden a crear.</param>
	/// <param name="userId">ID del usuario que crea la orden.</param>
	/// <returns>La orden creada con su ID asignado.</returns>
	public async Task<OrderDto> CreateAsync(OrderDto orderDto, string userId)
	{
		using var transaction = await _context.Database.BeginTransactionAsync();

		try
		{
			// Crear la entidad de orden
			var orderEntity = new OrderEntity
			{
				OrderNote = orderDto.OrderNote,
				OrderDate = DateTime.Now,
				TotalAmount = orderDto.TotalAmount,
				Status = orderDto.Status,
				UserId = userId,
				Details = new List<OrderDetailEntity>()
			};

			// Agregar detalles de la orden
			foreach (var detail in orderDto.Details)
			{
				// Verificar si hay suficiente stock del producto
				var product = await _context.Products.FindAsync(detail.ProductId);
				if (product == null || product.Stock < detail.Quantity)
				{
					throw new InvalidOperationException($"Stock insuficiente para el producto con ID {detail.ProductId}");
				}

				// Reducir el stock del producto
				product.Stock -= detail.Quantity;

				// Agregar detalle a la orden
				orderEntity.Details.Add(new OrderDetailEntity
				{
					ProductId = detail.ProductId,
					Quantity = detail.Quantity,
					UnitPrice = detail.UnitPrice
				});
			}

			// Guardar orden en la base de datos
			_context.Orders.Add(orderEntity);
			await _context.SaveChangesAsync();

			// Confirmar transacción
			await transaction.CommitAsync();

			// Actualizar el DTO con el ID generado
			orderDto.Id = orderEntity.Id;

			return orderDto;
		}
		catch (Exception)
		{
			// Si algo falla, revertir la transacción
			await transaction.RollbackAsync();
			throw;
		}
	}

	/// <summary>
	/// Obtiene todas las órdenes de la base de datos.
	/// </summary>
	/// <returns>Lista de órdenes.</returns>
	public async Task<IEnumerable<OrderDto>> GetAllAsync()
	{
		var orders = await _context.Orders
			.Include(o => o.Details)
			.ThenInclude(od => od.Product)
			.OrderByDescending(o => o.OrderDate)
			.ToListAsync();

		return orders.Select(MapEntityToDto);
	}

	/// <summary>
	/// Obtiene una orden por su ID.
	/// </summary>
	/// <param name="id">ID de la orden a buscar.</param>
	/// <returns>La orden si existe, null en caso contrario.</returns>
	public async Task<OrderDto?> GetAsync(int id)
	{
		var order = await _context.Orders
			.Include(o => o.Details)
			.ThenInclude(od => od.Product)
			.FirstOrDefaultAsync(o => o.Id == id);

		return order != null ? MapEntityToDto(order) : null;
	}

	/// <summary>
	/// Obtiene todas las órdenes de un usuario específico.
	/// </summary>
	/// <param name="userId">ID del usuario.</param>
	/// <returns>Lista de órdenes del usuario.</returns>
	public async Task<IEnumerable<OrderDto>> GetByUserIdAsync(string userId)
	{
		var orders = await _context.Orders
			.Include(o => o.Details)
			.ThenInclude(od => od.Product)
			.Where(o => o.UserId == userId)
			.OrderByDescending(o => o.OrderDate)
			.ToListAsync();

		return orders.Select(MapEntityToDto);
	}

	/// <summary>
	/// Actualiza el estado de una orden existente.
	/// </summary>
	/// <param name="id">ID de la orden a actualizar.</param>
	/// <param name="status">Nuevo estado de la orden.</param>
	/// <returns>La orden actualizada si existe, null en caso contrario.</returns>
	public async Task<OrderDto?> UpdateStatusAsync(int id, string status)
	{
		var order = await _context.Orders.FindAsync(id);
		if (order == null)
		{
			return null;
		}

		order.Status = status;
		await _context.SaveChangesAsync();

		return await GetAsync(id);
	}

	/// <summary>
	/// Cancela una orden y revierte el stock de los productos.
	/// </summary>
	/// <param name="id">ID de la orden a cancelar.</param>
	/// <returns>True si la orden fue cancelada, false en caso contrario.</returns>
	public async Task<bool> CancelOrderAsync(int id)
	{
		using var transaction = await _context.Database.BeginTransactionAsync();

		try
		{
			var order = await _context.Orders
				.Include(o => o.Details)
				.FirstOrDefaultAsync(o => o.Id == id);

			if (order == null || order.Status == "Cancelled")
			{
				return false;
			}

			// Restaurar stock de productos
			foreach (var detail in order.Details)
			{
				var product = await _context.Products.FindAsync(detail.ProductId);
				if (product != null)
				{
					product.Stock += detail.Quantity;
				}
			}

			// Actualizar estado de la orden
			order.Status = "Cancelled";
			await _context.SaveChangesAsync();

			// Confirmar transacción
			await transaction.CommitAsync();

			return true;
		}
		catch (Exception)
		{
			// Si algo falla, revertir la transacción
			await transaction.RollbackAsync();
			throw;
		}
	}

	/// <summary>
	/// Convierte una entidad de orden a su DTO correspondiente.
	/// </summary>
	private OrderDto MapEntityToDto(OrderEntity entity)
	{
		return new OrderDto
		{
			Id = entity.Id,
			OrderNote = entity.OrderNote,
			OrderDate = entity.OrderDate,
			TotalAmount = entity.TotalAmount,
			Status = entity.Status,
			UserId = entity.UserId,
			Details = entity.Details.Select(od => new OrderDetailDto
			{
				Id = od.Id,
				OrderId = od.OrderId,
				ProductId = od.ProductId,
				Quantity = od.Quantity,
				UnitPrice = od.UnitPrice,
				Product = od.Product != null ? new ProductDto
				{
					Id = od.Product.Id,
					Name = od.Product.Name,
					Description = od.Product.Description ?? string.Empty,
					Price = od.Product.Price,
					Stock = od.Product.Stock,
					Image = od.Product.Image,
					Active = od.Product.Active,
					Type = od.Product.Type
				} : null
			}).ToList()
		};
	}
}