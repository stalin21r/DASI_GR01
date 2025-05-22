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

            if (entity == null) return null;
            
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
        public async Task<OrderDto> UpDateAsync()
        {
            // Si el status de la orden es Pendiente, actualizar.
            // Si el status de la orden es Pagada, crear una nueva.

            return null;
        }

        // Eliminar una orden
        public async Task<bool> DeleteAsync(int orderId)
        {
            // Cambiar la variable IsActive como falsa
            return false;
        }
    }
}
