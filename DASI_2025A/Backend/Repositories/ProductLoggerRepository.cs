using Microsoft.AspNetCore.Http.HttpResults;
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
		var logs = await _context.ProductLogs
		.Include(l => l.Product)
		.Include(l => l.User)
		.AsNoTracking()
		.OrderByDescending(l => l.Id)
		.ToListAsync();
		if (logs is null || logs.Count() == 0)
		{
			throw new KeyNotFoundException("No se encontraron logs de productos.");
		}

		var logsDto = new List<ProductLoggerDto>();

		foreach (var log in logs)
		{
			var logDto = new ProductLoggerDto
			{
				Id = log.Id,
				UserId = log.User!.Id,
				UserName = log.User!.Email,
				ProductId = log.Product!.Id,
				ProductName = log.Product!.Name,
				Action = log.Action,
				Description = log.Description,
				QuantityBefore = log.QuantityBefore,
				QuantityAfter = log.QuantityAfter,
				AuditableDate = log.AuditableDate
			};
			logsDto.Add(logDto);
		}
		if (logsDto is null || logsDto.Count() == 0)
		{
			throw new BadHttpRequestException("Error al obtener logs de productos");
		}

		return logsDto;
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

		_context.ProductLogs.Add(entity);
		await _context.SaveChangesAsync();

		return productLoggerDto;
	}

}
