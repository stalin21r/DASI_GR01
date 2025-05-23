﻿using Shared;

namespace Backend;

public interface IProductLoggerService
{
	Task<ApiResponse<IEnumerable<ProductLoggerDto>>> GetAllProductLogsAsync();
	Task<ApiResponse<ProductLoggerDto>> GetProductLogByIdAsync(int id);
	Task<ApiResponse<IEnumerable<ProductLoggerDto>>> GetProductLogsByProductIdAsync(int productId);
}
