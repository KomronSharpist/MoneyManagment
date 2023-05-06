using MoneyManagment.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace MoneyManagment.Service.DTOs.TransactionCategory;

public class TransactionCategoryCreationDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    public TransactionType TransactionType { get; set; }
}
