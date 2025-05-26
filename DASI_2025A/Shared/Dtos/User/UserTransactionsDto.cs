namespace Shared;

public class UserTransactionsDto
{
  public string UserId { get; set; } = string.Empty;
  public string FirstName { get; set; } = string.Empty;
  public string LastName { get; set; } = string.Empty;
  public decimal Balance { get; set; }
  public List<BalanceTransactionDto> Transactions { get; set; } = new();
}