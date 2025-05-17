using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared;
public class WalletDto
{
	public int Id { get; set; }
	public string Action { get; set; }
	public string Status { get; set; }
	public decimal Amount { get; set; }
	public int OrderId { get; set; }
	public DateTime AuditableDate { get; set; }
}