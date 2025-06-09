using System.ComponentModel.DataAnnotations;

namespace Shared;

public class TopUpRequestResponseDto
{
  public int Id { get; set; }
  public decimal Amount { get; set; }
  public string Type { get; set; } = null!;
  public string Status { get; set; } = null!;
  public string? Receipt { get; set; }
  public string? TargetUser { get; set; } = null!;
  public string? RequestedByUser { get; set; } = null!;
  public string? AuthorizedByUser { get; set; }
  public DateTime AuditableDate { get; set; }
  public string? MachineName { get; set; }
}