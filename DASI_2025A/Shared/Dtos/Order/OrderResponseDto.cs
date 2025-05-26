namespace Shared;

public class OrderResponseDto
{
  public int Id { get; set; }

  public string BuyerId { get; set; } = null!;

  public string? BuyerFullName { get; set; }

  public string SellerId { get; set; } = null!;

  public string? SellerFullName { get; set; }

  public decimal Total { get; set; }

  public DateTime CreatedAt { get; set; }

  public List<OrderDetailResponseDto> OrderDetails { get; set; } = new();
}
