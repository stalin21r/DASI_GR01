using Microsoft.EntityFrameworkCore;
using Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend
{
    public class OrderRepository
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

            // Faltan validaciones

            _context.Orders.Add(order);
            var result = await _context.SaveChangesAsync();

            return orderDto;
        }

        // Leer ordenes
        public async Task<IEnumerable<OrderDto>> GetOrdersAsync()
        {
            // Faltan validaciones
            var orders = await _context.Orders.Include(o => o.Details).ToListAsync();

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
        public async Task<OrderDto> GetAsync(int id)
        {
            // Faltan validaciones
            var entity = await _context.Orders.Include(o => o.Details).FirstOrDefaultAsync(o => o.Id == id);

            if (entity == null)
            {
                throw new KeyNotFoundException("No se encontró la orden");
            }

            var order = new OrderDto
            {
                Id = entity.Id,
                OrderNote = entity.OrderNote,
                OrderDate = entity.OrderDate,
                TotalAmount = entity.TotalAmount,
                Status = entity.Status,
                UserId = entity.UserId,
                IsReverted = entity.IsReverted,
                Details = entity.Details.Select(dto => new OrderDetailDto
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    UnitPrice = dto.UnitPrice,
                    SubTotal = dto.SubTotal,
                    OrderId = dto.OrderId
                }).ToList()
            };

            return order;
        } 

        // Actualizar una orden
        public async Task UpDateAsync(int orderId)
        {
            // Solo actualizar si el estado de la orden es Pendiente
            var entity = await _context.Orders.FindAsync(orderId);
            if (entity == null)
            {
                throw new KeyNotFoundException("No se encontró la orden");
            }

            if (entity.Status == Status.Paid)
            {
                throw new InvalidOperationException("No se puede modificar una orden que ya fue pagada");
            }

            // Falta la logica para actualizar

            // return null;
        }

        // Metodo CancelOrderAync para poder cancelar una orden
        public async Task CancelOrderAsync(int orderId)
        {
            var entity = await _context.Orders.FindAsync(orderId);
            if (entity == null)
            {
                throw new KeyNotFoundException("No se encontró la orden");
            }

            if (entity.Status == Status.Paid)
            {
                throw new InvalidOperationException("No se puede cancelar una orden que ya fue pagada");
            }

            entity.Status = Status.Cancelled;
            await _context.SaveChangesAsync();
        }

        // Eliminar una orden
        public async Task SoftDeleteAsync(int orderId)
        {
            var entity = await _context.Orders.FindAsync(orderId);
            if (entity == null)
            {
                throw new KeyNotFoundException("No se encontró la orden.");
            }

            if (entity.Status == Status.Paid)
            {
                throw new InvalidOperationException("No se puede eliminar una orden que ya fue pagada");
            }

            entity.IsActive = false;
            await _context.SaveChangesAsync();
        }
    }
}
