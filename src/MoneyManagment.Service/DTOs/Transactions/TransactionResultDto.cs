using MoneyManagment.Domain.Enums;

namespace MoneyManagment.Service.DTOs.Transactions;

public class TransactionResultDto
{
    public long Id { get; set; }
    public TransactionType TransactionType { get; set; }
    public long CategoryId { get; set; }
    public decimal Sum { get; set; }
    public string Description { get; set; }
    public long UserId { get; set; }
}
