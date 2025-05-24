using Microsoft.EntityFrameworkCore;
using Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository (ApplicationDbContext context)
        {
            _context = context;
        }

        //  ===== Metodos de Orden ====

        // Crear una orden
        public async Task<OrderDto> CreateAsync(OrderDto orderDto)
        {
            var order = new OrderEntity
            {
                OrderNote = orderDto.OrderNote,
                OrderDate = orderDto.OrderDate,
                TotalAmount = orderDto.TotalAmount,
                UserId = orderDto.UserId,
                Details = orderDto.Details.Select(dto => new OrderDetailEntity { 
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    UnitPrice = dto.UnitPrice,
                    SubTotal = dto.SubTotal,
                    OrderId = dto.OrderId
                }).ToList()

            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return new OrderDto
            {
                Id = order.Id,
                OrderNote = order.OrderNote,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                UserId = order.UserId,
                Details = order.Details.Select(dto => new OrderDetailDto
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    UnitPrice = dto.UnitPrice,
                    SubTotal = dto.SubTotal,
                    OrderId = dto.OrderId
                }).ToList()
            };
        }

        // Leer ordenes
        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            // Faltan validaciones
            var orders = await _context.Orders
                .Include(o => o.Details)
                .ToListAsync();

            var response = orders.Select(
                order => new OrderDto
                {
                    Id = order.Id,
                    OrderNote = order.OrderNote,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status,
                    UserId = order.UserId,
                    IsReverted = order.IsReverted,
                    Details = order.Details.Select(dto => new OrderDetailDto
                    {
                        ProductId = dto.ProductId,
                        Quantity = dto.Quantity,
                        UnitPrice = dto.UnitPrice,
                        SubTotal = dto.SubTotal,
                        OrderId = dto.OrderId
                    }).ToList()
                }
            );

            return response.ToList();
        }


        // Leer una orden
        public async Task<OrderEntity?> GetOrderAsync(int id)
        {
            return await _context.Orders.Include(o => o.Details).FirstOrDefaultAsync(o => o.Id == id);

            // Se necesitan filtrar las ordenes canceladas?
        } 

        // Actualizar una orden
        public async Task UpdateOrderAsync(OrderEntity orderEntity)
        {
            // Si está 'attached' y se le cambió el Status en la capa de servicio
            await _context.SaveChangesAsync();

        }

        // Metodo CancelOrderAync para poder cancelar una orden
        public async Task CancelOrderAsync(OrderEntity orderEntity)
        {
            // Si está 'attached' y se le cambió el Status en la capa de servicio
            await _context.SaveChangesAsync();
        }

        // Eliminar una orden
        public async Task SoftDeleteOrderAsync(OrderEntity orderEntity)
        {
            // Si está 'attached' y se le cambió el Status en la capa de servicio
            await _context.SaveChangesAsync();
        }
    }
}
