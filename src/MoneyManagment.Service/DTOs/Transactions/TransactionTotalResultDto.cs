namespace MoneyManagment.Service.DTOs.Transactions;

public class TransactionTotalResultDto
{
    public decimal Total { get; set; }
    public long? UserId { get; set; }
    public decimal Income { get; set; }
    public decimal Outgo { get; set; }
}
