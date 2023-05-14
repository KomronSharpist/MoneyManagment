using MoneyManagment.Service.DTOs.Transactions;

namespace MoneyManagmen.Web.Models
{
    public class TotalAndTransaction
    {
        public TransactionTotalResultDto TotalResultDto { get; set; }
        public List<TransactionResultDto> TransactionResults { get; set; }
    }
}
