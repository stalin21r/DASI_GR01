using Microsoft.EntityFrameworkCore;
using Shared;

namespace Backend;

public class OrderRepository : IOrderRepository
{
  private readonly ApplicationDbContext _context;
  private readonly IProductRepository _productRepository;
  public OrderRepository(ApplicationDbContext context, IProductRepository productRepository)
  {
    _context = context;
    _productRepository = productRepository;
  }
  /// <summary>
  /// Crea una orden de compra y registra la transacción en la base de datos.
  /// </summary>
  /// <param name="order">La orden de compra a crear.</param>
  /// <returns>La orden creada con los nombres de los comprador y vendedor.</returns>
  /// <exception cref="KeyNotFoundException">Si no se encontró el comprador.</exception>
  /// <exception cref="InvalidOperationException">Si el saldo del comprador es insuficiente o no se pudo realizar la venta.</exception>
  public async Task<OrderResponseDto> CreateOrderAsync(OrderCreateDto order)
  {
    var buyer = await _context.Users.FirstOrDefaultAsync(u => u.Id == order.BuyerId);
    if (buyer == null || buyer.Active == false)
    {
      throw new KeyNotFoundException("No se encontró o no existe el comprador.");
    }
    //validacion que el balance solo puede restarse hasta llegar a -10
    decimal finalBalance = buyer.Balance - order.Total;
    if (finalBalance < -10)
    {
      throw new InvalidOperationException("Saldo insuficiente.");
    }
    bool sellOk = await _productRepository.RegisterSellProductAsync(order, order.BuyerId);
    if (!sellOk)
    {
      throw new InvalidOperationException("No se pudo realizar la venta.");
    }
    var orderEntity = new OrderEntity
    {
      BuyerId = order.BuyerId,
      SellerId = order.SellerId,
      Total = order.Total,
      OrderDetails = order.OrderDetails.Select(od => new OrderDetailEntity
      {
        ProductId = od.ProductId,
        Quantity = od.Quantity,
        UnitPrice = od.UnitPrice,
        Subtotal = od.Subtotal
      }).ToList()
    };
    await _context.Orders.AddAsync(orderEntity);
    var transaction = new BalanceTransactionsEntity
    {
      UserId = order.BuyerId,
      Type = "DÉBITO",
      Amount = order.Total,
      Description = "Compra de productos",
      BalanceBefore = buyer.Balance,
      BalanceAfter = finalBalance
    };
    await _context.BalanceTransactions.AddAsync(transaction);
    buyer.Balance = finalBalance;
    await _context.SaveChangesAsync();
    // Obtener nombres para el DTO de respuesta (puede optimizarse si ya están en el contexto)
    var seller = await _context.Users.FirstOrDefaultAsync(u => u.Id == order.SellerId);
    return new OrderResponseDto
    {
      Id = orderEntity.Id,
      BuyerId = order.BuyerId,
      BuyerFullName = $"{buyer.FirstName} {buyer.LastName}",
      SellerId = order.SellerId,
      SellerFullName = seller != null ? $"{seller.FirstName} {seller.LastName}" : null,
      Total = order.Total,
      CreatedAt = orderEntity.AuditableDate,
      OrderDetails = orderEntity.OrderDetails.Select(d => new OrderDetailResponseDto
      {
        Id = d.Id,
        ProductId = d.ProductId,
        Quantity = d.Quantity,
        UnitPrice = d.UnitPrice,
        Subtotal = d.Subtotal,
        // Opcional: incluir nombre o imagen del producto si lo deseas aquí
      }).ToList()
    };
  }
  /// <summary>
  /// Obtiene todas las órdenes de compra registradas en el sistema.
  /// </summary>
  /// <returns>Una lista de DTOs que representan todas las órdenes de compra, incluyendo detalles del comprador, vendedor y productos.</returns>
  /// <exception cref="KeyNotFoundException">Si no se encontraron órdenes.</exception>
  public async Task<(IEnumerable<OrderResponseDto> Orders, int TotalItems)> GetAllOrdersAsync(OrderQueryParams queryParams)
  {
    var query = _context.Orders
      .Include(o => o.Buyer)
      .Include(o => o.Seller)
      .Include(o => o.OrderDetails)
      .ThenInclude(od => od.Product)
      .AsQueryable();

    // Aplicar filtros
    if (queryParams.StartDate.HasValue)
      query = query.Where(o => o.AuditableDate >= queryParams.StartDate.Value);

    if (queryParams.EndDate.HasValue)
      query = query.Where(o => o.AuditableDate <= queryParams.EndDate.Value);

    if (!string.IsNullOrWhiteSpace(queryParams.BuyerFullName))
      query = query.Where(o => (o.Buyer!.FirstName + " " + o.Buyer.LastName).Contains(queryParams.BuyerFullName));

    if (!string.IsNullOrWhiteSpace(queryParams.SellerFullName))
      query = query.Where(o => (o.Seller!.FirstName + " " + o.Seller.LastName).Contains(queryParams.SellerFullName));

    var totalItems = await query.CountAsync();

    var orders = await query
      .OrderByDescending(o => o.Id)
      .Skip(queryParams.Skip)
      .Take(queryParams.PageSize)
      .ToListAsync();

    var result = orders.Select(order => new OrderResponseDto
    {
      Id = order.Id,
      BuyerId = order.BuyerId,
      BuyerFullName = $"{order.Buyer?.FirstName} {order.Buyer?.LastName}",
      SellerId = order.SellerId,
      SellerFullName = $"{order.Seller?.FirstName} {order.Seller?.LastName}",
      Total = order.Total,
      CreatedAt = order.AuditableDate,
      OrderDetails = order.OrderDetails.Select(detail => new OrderDetailResponseDto
      {
        Id = detail.Id,
        ProductId = detail.ProductId,
        Quantity = detail.Quantity,
        UnitPrice = detail.UnitPrice,
        Subtotal = detail.Subtotal,
        ProductName = detail.Product!.Name
      }).ToList()
    });

    return (result, totalItems);
  }

  /// <summary>
  /// Obtiene todas las ordenes de un usuario en su rol de comprador.
  /// </summary>
  /// <param name="userId">El ID del usuario comprador.</param>
  /// <returns>Una lista de DTOs que representan las ordenes del comprador.</returns>
  /// <exception cref="KeyNotFoundException">Si no se encontraron ordenes.</exception>

  public async Task<IEnumerable<OrderResponseDto>> GetOrdersByBuyerIdAsync(string userId)
  {
    var orders = await _context.Orders
      .Where(o => o.BuyerId == userId)
      .Include(o => o.Buyer)
      .Include(o => o.Seller)
      .Include(o => o.OrderDetails)
      .ThenInclude(od => od.Product)
      .OrderByDescending(o => o.Id)
      .ToListAsync();
    if (orders == null)
    {
      throw new KeyNotFoundException("No se encontraron ordenes.");
    }
    return orders.Select(order => new OrderResponseDto
    {
      Id = order.Id,
      BuyerId = order.BuyerId,
      BuyerFullName = $"{order.Buyer?.FirstName} {order.Buyer?.LastName}",
      SellerId = order.SellerId,
      SellerFullName = $"{order.Seller?.FirstName} {order.Seller?.LastName}",
      Total = order.Total,
      CreatedAt = order.AuditableDate,
      OrderDetails = order.OrderDetails.Select(detail => new OrderDetailResponseDto
      {
        Id = detail.Id,
        ProductId = detail.ProductId,
        Quantity = detail.Quantity,
        UnitPrice = detail.UnitPrice,
        Subtotal = detail.Subtotal,
      }).ToList()
    });
  }
  /// <summary>
  ///   Obtiene todas las ordenes de un usuario en su rol de vendedor.
  /// </summary>
  /// <param name="userId">El ID del usuario vendedor.</param>
  /// <returns>Una lista de DTOs que representan las ordenes del vendedor.</returns>
  /// <exception cref="KeyNotFoundException">Si no se encontraron ordenes.</exception>
  public async Task<IEnumerable<OrderResponseDto>> GetOrdersBySellerIdAsync(string userId)
  {
    var orders = await _context.Orders
      .Where(o => o.SellerId == userId)
      .Include(o => o.Buyer)
      .Include(o => o.Seller)
      .Include(o => o.OrderDetails)
      .ThenInclude(od => od.Product)
      .OrderByDescending(o => o.Id)
      .ToListAsync();
    if (orders == null)
    {
      throw new KeyNotFoundException("No se encontraron ordenes.");
    }
    return orders.Select(order => new OrderResponseDto
    {
      Id = order.Id,
      BuyerId = order.BuyerId,
      BuyerFullName = $"{order.Buyer?.FirstName} {order.Buyer?.LastName}",
      SellerId = order.SellerId,
      SellerFullName = $"{order.Seller?.FirstName} {order.Seller?.LastName}",
      Total = order.Total,
      CreatedAt = order.AuditableDate,
      OrderDetails = order.OrderDetails.Select(detail => new OrderDetailResponseDto
      {
        Id = detail.Id,
        ProductId = detail.ProductId,
        Quantity = detail.Quantity,
        UnitPrice = detail.UnitPrice,
        Subtotal = detail.Subtotal,
      }).ToList()
    });
  }
}