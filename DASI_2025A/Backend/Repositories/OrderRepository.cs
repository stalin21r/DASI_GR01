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
	/// <summary>
	/// Crea una orden para la venta de un producto específico
	/// </summary>
	public async Task<OrderDto> CreateSaleOrderAsync(int productId, uint quantity, string userId)
	{
		using var transaction = await _context.Database.BeginTransactionAsync();

		try
		{
			// Obtener el producto
			var product = await _context.Products.FindAsync(productId);
			if (product == null)
				throw new KeyNotFoundException($"No se encontró el producto con ID {productId}");

			if (!product.Active)
				throw new InvalidOperationException("No se puede vender un producto inactivo");

			if (product.Stock < quantity)
				throw new InvalidOperationException($"Stock insuficiente para {product.Name}. Disponible: {product.Stock}, Solicitado: {quantity}");

			// Reducir el stock del producto
			/*product.Stock -= quantity;*/

			// Crear la entidad de orden
			var orderEntity = new OrderEntity
			{
				OrderNote = $"Venta-{Guid.NewGuid()}",
				OrderDate = DateTime.Now,
				TotalAmount = product.Price * quantity,
				Status = "Confirmado",
				UserId = userId,
				Details = new List<OrderDetailEntity>
			{
				new OrderDetailEntity
				{
					ProductId = productId,
					Quantity = quantity,
					UnitPrice = product.Price
				}
			}
			};

			// Guardar cambios
			_context.Orders.Add(orderEntity);
			await _context.SaveChangesAsync();

			// Confirmar transacción
			await transaction.CommitAsync();

			// Mapear a DTO
			return MapEntityToDto(orderEntity);
		}
		catch (Exception)
		{
			// Si algo falla, revertir la transacción
			await transaction.RollbackAsync();
			throw;
		}
	}
	/// <summary>
	/// Crea una orden específica para recargas de saldo
	/// </summary>
	public async Task<OrderDto> CreateTopUpOrderAsync(decimal amount, string userId, string description)
	{
		// Crear la entidad de orden para recarga
		var orderEntity = new OrderEntity
		{
			OrderNote = "Recarga de Saldo desde: " + description,
			OrderDate = DateTime.Now,
			TotalAmount = amount,
			Status = "Completado",
			UserId = userId,
			Details = new List<OrderDetailEntity>() // Sin detalles de productos
		};

		// Agregar la orden al contexto
		_context.Orders.Add(orderEntity);
		await _context.SaveChangesAsync();

		// Mapear a DTO y devolver
		return new OrderDto
		{
			Id = orderEntity.Id,
			OrderNote = orderEntity.OrderNote,
			OrderDate = orderEntity.OrderDate,
			TotalAmount = orderEntity.TotalAmount,
			Status = orderEntity.Status,
			UserId = orderEntity.UserId,
			Details = new List<OrderDetailDto>() // Sin detalles
		};
	}
}