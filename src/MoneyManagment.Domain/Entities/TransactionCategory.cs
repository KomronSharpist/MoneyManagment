using MoneyManagment.Domain.Commons;
using MoneyManagment.Domain.Enums;

namespace MoneyManagment.Domain.Entities;

public class TransactionCategory : Auditable
{
    public string Name { get; set; }
    public TransactionType TransactionType { get; set; }
}
