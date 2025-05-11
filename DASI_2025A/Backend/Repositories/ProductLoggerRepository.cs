using Microsoft.EntityFrameworkCore;
using Shared;


namespace Backend;

public class ProductLoggerRepository : IProductLoggerRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductLoggerRepository(ApplicationDbContext context)
        {
            _context = context;
        }
		public async Task<IEnumerable<ProductLoggerDto>> GetAllAsync()
		{
			return await _context.ProductLogs
				.Select(p => new ProductLoggerDto
				{
					Id = p.Id,
					Action = p.Action,
					Description = p.Description,
					QuantityBefore = p.QuantityBefore,
					QuantityAfter = p.QuantityAfter,
					ProductId = p.ProductFk,
					UserId = p.UserFk
				})
				.ToListAsync();
		}

		public async Task<ProductLoggerDto?> GetAsync(int id)
		{
			return await _context.ProductLogs
				.Select(p => new ProductLoggerDto
				{
					Id = p.Id,
					Action = p.Action,
					Description = p.Description,
					QuantityBefore = p.QuantityBefore,
					QuantityAfter = p.QuantityAfter,
					ProductId = p.ProductFk,
					UserId = p.UserFk
				})
				.FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<ProductLoggerDto>> GetByProductIdAsync(int productId)
		{
			return await _context.ProductLogs.Where(p => p.ProductFk == productId).Select(
				p => new ProductLoggerDto
				{
					Id = p.Id,
					Action = p.Action,
					Description = p.Description,
					QuantityBefore = p.QuantityBefore,
					QuantityAfter = p.QuantityAfter,
					ProductId = p.ProductFk,
					UserId = p.UserFk
				})
				.ToListAsync();
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
