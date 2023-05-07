using System.ComponentModel.DataAnnotations;

namespace MoneyManagment.Service.DTOs.Transactions;
public class TransactionUpdateDto
{
    [Required]
    public long Id { get; set; }
    [Required]
    public long CategoryId { get; set; }
    [Required]
    public decimal Sum { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow.ToUniversalTime();
}
