using Humanizer;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Backend
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;
        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaymentDto> CreateAsync(PaymentDto paymentDto) // Pago de ordenes
        {
            var payment = new PaymentEntity
            {
                AmountDue = paymentDto.AmountDue,
                PaymentMethod = paymentDto.PaymentMethod,
                Status = paymentDto.Status,
                OrderId = paymentDto.OrderId,
            };

            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();

            return new PaymentDto
            {
                Id = payment.Id,
                AmountDue = payment.AmountDue,
                PaymentMethod = payment.PaymentMethod,
                Status = payment.Status,
                IssuedAt = payment.IssuedAt,
                OrderId = payment.OrderId
            };
        }

        public async Task<IEnumerable<PaymentDto>> GetAllAsync()
        {
            var payments = await _context.Payments
                .Select(payment => new PaymentDto
                {
                    Id = payment.Id,
                    AmountDue = payment.AmountDue,
                    PaymentMethod = payment.PaymentMethod,
                    Status = payment.Status,
                    IssuedAt= payment.IssuedAt,
                    OrderId = payment.OrderId
                })
                .ToListAsync();

            return payments;
        }

        public async Task<PaymentEntity?> GetAsync(int paymentId)
        {
           return await _context.Payments.FindAsync(paymentId);
        }

        public async Task UpdateAsync(PaymentEntity paymentEntity)
        {
            // Si está 'attached' y se le cambió el Status en la capa de servicio
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(PaymentEntity paymentEntity)
        {
            await _context.SaveChangesAsync();
        }
       
    }
}
