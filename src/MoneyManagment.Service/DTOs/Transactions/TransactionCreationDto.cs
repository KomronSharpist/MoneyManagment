using MoneyManagment.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace MoneyManagment.Service.DTOs.Transactions;

public class TransactionCreationDto
{
    [Required]
    public TransactionType TransactionType { get; set; }
    [Required]
    public long CategoryId { get; set; }
    [Required]
    public decimal Sum { get; set; }
    [Required]
    public string Description { get; set; }
}
