using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Backend
{
    public class OrderService : IOrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly UserRepository _userRepository;
        
        public OrderService(OrderRepository orderRepository, UserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<OrderDto>> CreateOrderAsync(OrderDto orderDto)
        {
            var user = await _userRepository.GetAsync(orderDto.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException("No se encontro al Usuario para crear la Orden");
            }

            var response = await _orderRepository.CreateAsync(orderDto);
            if (response == null)
            {
                throw new BadHttpRequestException("Error al crear la orden");
            }

            ApiResponse<OrderDto> result = new ApiResponse<OrderDto>(
                message: "Orden creada exitosamente",
                data: response,
                totalRecords: 1
            );

            return result;
        }
    }
}
