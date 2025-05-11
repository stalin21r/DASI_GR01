using Shared;

namespace Backend.Services
{
    public class ProductLoggerService : IProductLoggerService
    {
        private readonly IProductLoggerRepository _repository;

        public async Task<ApiResponse<ProductLoggerDto>> CreateProductLogAsync(ProductLoggerDto productLoggerDto)
        {
            var result = await _repository.CreateAsync(productLoggerDto);
            if (result == null)
            {
                throw new BadHttpRequestException("Error al crear el productLogger.");
            }
            ApiResponse<ProductLoggerDto> response = new ApiResponse<ProductLoggerDto>(
              message: "Log de producto creado exitosamente",
              data: result,
              totalRecords: 1
            );
            return response;
        }
    }
}
