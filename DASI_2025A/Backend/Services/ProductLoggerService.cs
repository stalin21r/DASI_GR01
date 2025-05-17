using Shared;

namespace Backend;
public class ProductLoggerService : IProductLoggerService
{
	private readonly IProductLoggerRepository _repository;

	public ProductLoggerService(IProductLoggerRepository repository)
	{
		_repository = repository;
	}

    //public async Task<ApiResponse<ProductLoggerDto>> CreateProductLogAsync(ProductLoggerDto productLoggerDto)
    //{
    //    var result = await _repository.CreateAsync(productLoggerDto);
    //    if (result == null)
    //    {
    //        throw new BadHttpRequestException("Error al crear el productLogger.");
    //    }
    //    ApiResponse<ProductLoggerDto> response = new ApiResponse<ProductLoggerDto>(
    //      message: "Log de producto creado exitosamente",
    //      data: result,
    //      totalRecords: 1
    //    );
    //    return response;
    //}

    public async Task<ApiResponse<IEnumerable<ProductLoggerDto>>> GetAllProductLogsAsync()
	{
		var logs = await _repository.GetAllAsync();

		ApiResponse<IEnumerable<ProductLoggerDto>> response = new ApiResponse<IEnumerable<ProductLoggerDto>>(
			message: "Logs de productos obtenidos exitosamente",
			data: logs,
			totalRecords: logs.Count()
		);

		return response;
	}
	public async Task<ApiResponse<ProductLoggerDto>> GetProductLogByIdAsync(int id)
	{
		var log = await _repository.GetAsync(id);

		if (log == null)
		{
			throw new BadHttpRequestException($"No se encontró un log con el ID {id}.");
		}

		ApiResponse<ProductLoggerDto> response = new ApiResponse<ProductLoggerDto>(
			message: "Log de producto obtenido exitosamente",
			data: log,
			totalRecords: 1
		);

		return response;
	}

	public async Task<ApiResponse<IEnumerable<ProductLoggerDto>>> GetProductLogsByProductIdAsync(int productId)
	{
		var logs = await _repository.GetByProductIdAsync(productId);

		ApiResponse<IEnumerable<ProductLoggerDto>> response = new ApiResponse<IEnumerable<ProductLoggerDto>>(
			message: $"Logs del producto con ID {productId} obtenidos exitosamente",
			data: logs,
			totalRecords: logs.Count()
		);

		return response;
	}


}
