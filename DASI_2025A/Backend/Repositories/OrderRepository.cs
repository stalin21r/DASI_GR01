using Microsoft.EntityFrameworkCore;
using Shared;

namespace Backend;

public class OrderRepository : IOrderRepository
{
  private readonly ApplicationDbContext _context;
  private readonly IProductRepository _productRepository;
  private readonly IUserRepository _userRepository;
  public OrderRepository(ApplicationDbContext context, IProductRepository productRepository, IUserRepository userRepository)
  {
    _context = context;
    _productRepository = productRepository;
    _userRepository = userRepository;
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
  /// Obtiene todas las órdenes de compra
  /// </summary>
  /// <returns>Una lista de DTO de respuesta de órdenes de compra</returns>
  public async Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync()
  {
    var orders = await _context.Orders
      .Include(o => o.Buyer)
      .Include(o => o.Seller)
      .Include(o => o.OrderDetails)
      .ThenInclude(od => od.Product)
      .OrderByDescending(o => o.Id)
      .ToListAsync();

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
        // Puedes agregar más propiedades si tu DTO las tiene, como nombre o imagen del producto
      }).ToList()
    });
  }

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