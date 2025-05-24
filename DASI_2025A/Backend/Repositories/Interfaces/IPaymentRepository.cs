using Shared;

namespace Backend
{
    public interface IPaymentRepository
    {
        Task<PaymentDto> CreateAsync(PaymentDto paymentDto);
        Task<IEnumerable<PaymentDto>> GetAllAsync();
        Task<PaymentEntity?> GetAsync(int paymentId); // para uso interno
        Task UpdateAsync(PaymentEntity paymentEntity);
        Task DeleteAsync(PaymentEntity paymentEntity);
    }
}
