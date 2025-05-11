using Microsoft.EntityFrameworkCore;
using Shared;


namespace Backend
{
    public class ProductLoggerRepository : IProductLoggerRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductLoggerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductLoggerDto> CreateAsync(ProductLoggerDto productLoggerDto)
        {
            var entity = new ProductLoggerEntity
            {
                Action = productLoggerDto.Action,
                Description = productLoggerDto.Description,
                QuantityBefore = productLoggerDto.QuantityBefore,
                QuantityAfter = productLoggerDto.QuantityAfter,
                ProductFk = productLoggerDto.ProductId,
                UserFk = productLoggerDto.UserId
            };

            _context.Add(entity);
            await _context.SaveChangesAsync();

            return productLoggerDto;
        }


    }
}
