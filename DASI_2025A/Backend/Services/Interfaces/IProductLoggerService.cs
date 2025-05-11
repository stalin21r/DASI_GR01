using Shared;

namespace Backend
{
    public interface IProductLoggerService
    {
        Task<ApiResponse<ProductLoggerDto>> CreateProductLogAsync(ProductLoggerDto productLoggerDto);
    }
}
