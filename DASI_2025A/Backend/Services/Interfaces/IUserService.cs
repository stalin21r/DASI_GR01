﻿using Shared;

namespace Backend;

public interface IUserService
{
  Task<ApiResponse<UserDto>> CreateAsync(UserDto userDto);
  Task<ApiResponse<IEnumerable<UserDto>>> GetAllAsync();
  Task<ApiResponse<UserDto>> GetAsync(string id);
  Task<ApiResponse<UserDto>> GetByEmailAsync(string email);
  Task<ApiResponse<UserDto>> UpdateAsync(UserDto userDto);
  Task<ApiResponse<bool>> DeleteAsync(string id);
  Task<ApiResponse<UserTransactionsDto>> GetUserTransactionsAsync(string userId);
  Task<ApiResponse<TopUpRequestResponseDto>> CreateTopUpRequestAsync(TopUpRequestCreateDto topUpRequestDto);
  Task<ApiResponse<TopUpRequestResponseDto>> AproveOrRejectTopUpAsync(TopUpRequestUpdateDto topUpRequestDto);
  Task<ApiResponse<IEnumerable<TopUpRequestResponseDto>>> GetTopUpRequestsAsync();
  Task<ApiResponse<IEnumerable<TopUpRequestResponseDto>>> GetTopUpRequestsByUserIdAsync(string userId);
  Task<ApiResponse<UserProfileDto>> GetCurrentUserProfileAsync();
}
