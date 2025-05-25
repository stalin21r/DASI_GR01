using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Shared;

namespace Backend
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        
        public PaymentService(IPaymentRepository paymentRepository, IOrderRepository orderRepository)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
        }

        // Crear una orden de pago para recargar ek saldo de un usuario
        public async Task<ApiResponse<PaymentDto>> CreatePaymentAsync(PaymentDto paymentDto)
        {
            // Comprobar que la orden existe
            var order = await _orderRepository.GetOrderAsync(paymentDto.OrderId);

            if (order == null)
            {
                throw new KeyNotFoundException("No se encontro la orden para crear el Pago");
            }

            var response = await _paymentRepository.CreateAsync(paymentDto);
            ApiResponse<PaymentDto> result = new ApiResponse<PaymentDto>
            (
                message: "Pago creado exitosamente",
                data: response,
                totalRecords: 1

            );

            return result;
        }
    }
}
