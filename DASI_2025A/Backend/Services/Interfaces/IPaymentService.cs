using Shared;

namespace Backend
{
    public interface IPaymentService
    {
        Task<ApiResponse<PaymentDto>> CreatePaymentAsync(PaymentDto paymentDto);
    }
}
