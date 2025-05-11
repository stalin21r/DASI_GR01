using Shared;

namespace Backend
{
    public interface IProductLoggerRepository
    {
        Task<ProductLoggerDto> CreateAsync(ProductLoggerDto productLoggerdto);
    }
}
