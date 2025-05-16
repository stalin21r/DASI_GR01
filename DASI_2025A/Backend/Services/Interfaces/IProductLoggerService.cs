using Shared;

<<<<<<< HEAD
namespace Backend
{
    public interface IProductLoggerService
    {
        Task<ApiResponse<ProductLoggerDto>> CreateProductLogAsync(ProductLoggerDto productLoggerDto);
    }
=======
namespace Backend;

public interface IProductLoggerService
{
	Task<ApiResponse<IEnumerable<ProductLoggerDto>>> GetAllProductLogsAsync();
	Task<ApiResponse<ProductLoggerDto>> GetProductLogByIdAsync(int id);
	Task<ApiResponse<IEnumerable<ProductLoggerDto>>> GetProductLogsByProductIdAsync(int productId);
>>>>>>> origin/development
}
