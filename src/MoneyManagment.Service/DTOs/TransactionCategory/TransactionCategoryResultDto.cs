using MoneyManagment.Domain.Enums;

namespace MoneyManagment.Service.DTOs.TransactionCategory;

public class TransactionCategoryResultDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public TransactionType TransactionType { get; set; }
}
