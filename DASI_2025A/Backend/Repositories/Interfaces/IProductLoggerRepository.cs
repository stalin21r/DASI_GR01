using Shared;

namespace Backend;
   
public interface IProductLoggerRepository 
{
	Task<ProductLoggerDto> CreateAsync(ProductLoggerDto productLoggerdto);
	Task<IEnumerable<ProductLoggerDto>> GetAllAsync();	
	Task<ProductLoggerDto?> GetAsync(int id);
	Task<IEnumerable<ProductLoggerDto>> GetByProductIdAsync(int productId);
}
