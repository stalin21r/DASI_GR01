using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Backend
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<OrderDetailDto> CreateOrderDetailAsync(OrderDetailDto orderDetailDto)
        {
            var newOrderDetail = new OrderDetailEntity
            {
                Quantity = orderDetailDto.Quantity,
                UnitPrice = orderDetailDto.UnitPrice,
                SubTotal = orderDetailDto.SubTotal,
                OrderId = orderDetailDto.OrderId,
                ProductId = orderDetailDto.ProductId,
                ProductName = orderDetailDto.ProductName 
            };

            await _context.OrderDetails.AddAsync(newOrderDetail);
            await _context.SaveChangesAsync();

            return orderDetailDto;
        }

        public async Task<IEnumerable<OrderDetailDto>> GetAllOrdersDetailAsync()
        {
            var response = await _context.OrderDetails
                .Select(order => new OrderDetailDto
                {
                    Id = order.Id,
                    Quantity = order.Quantity,
                    UnitPrice = order.UnitPrice,
                    SubTotal = order.SubTotal,
                    OrderId = order.OrderId,
                    ProductId = order.ProductId,
                    ProductName = order.ProductName
                })
                .ToListAsync();

            if (!response.Any())
            {
                throw new InvalidOperationException("No se encontraron registros de orderDetails");
            }

            return response;

        }

        public async Task<OrderDetailDto> GetOrderDetailAsync(int orderDetailId)
        {
            var entity = await _context.OrderDetails.FindAsync(orderDetailId); // FindAync es mas rapido si se usa PK y no se necesita ninguna relacion
            if (entity == null)
            {
                throw new KeyNotFoundException("No se encontró el detalle de la orden");
            }

            var orderDetail = new OrderDetailDto
            {
                Id = entity.Id,
                Quantity = entity.Quantity,
                UnitPrice = entity.UnitPrice,
                SubTotal = entity.SubTotal,
                OrderId = entity.OrderId,
                ProductId = entity.ProductId,
                ProductName = entity.ProductName
            };

            return orderDetail;
        }

        public async Task UpdateOrderDetailAsync(OrderDetailDto orderDetailDto, int orderDetailId)
        {
            var entity = await _context.OrderDetails.FindAsync(orderDetailId);
            if (entity == null)
            {
                throw new KeyNotFoundException("No se encontró el detalle de la orden");
            }

            // Falta logica para actualizar
        }

        public async Task CancelOrderDetailAsync(int orderDetailId)
        {
            var entity = await _context.OrderDetails.FindAsync(orderDetailId);
            if (entity == null)
            {
                throw new KeyNotFoundException("No se encontró el detalle de la orden");
            }

            entity.IsActive = false;

            await _context.SaveChangesAsync();

            // Trabajar con IsActive y IsCanceled o solo con IsCanceled?
        }

        //public async Task SoftDeleteOrderDetailAsync(int orderDetailId)
        //{

        //}
    }
}
