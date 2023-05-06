using MoneyManagment.Domain.Commons;
using MoneyManagment.Domain.Enums;

namespace MoneyManagment.Domain.Entities;

public class Transaction : Auditable
{
    public TransactionType TransactionType { get; set; }
    public long TransactionCategoryId { get; set; }
    public TransactionCategory TransactionCategory { get; set; }
    public decimal Sum { get; set; }
    public string Description { get; set; }
    public long UserId { get; set; }
    public User User { get; set; }
}
