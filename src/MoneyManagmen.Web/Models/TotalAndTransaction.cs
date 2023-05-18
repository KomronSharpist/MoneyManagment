using MoneyManagment.Service.DTOs.TransactionCategory;
using MoneyManagment.Service.DTOs.Transactions;

namespace MoneyManagmen.Web.Models
{
    public class TotalAndTransaction
    {
        public TransactionTotalResultDto TotalResultDto { get; set; }
        public List<TransactionCategoryResultDto> TransactionCategoryResultDto { get; set; }
        public List<TransactionResultDto> TransactionResults { get; set; }
        public TransactionCreationDto TransactionCreationDto { get; set; }
    }
}
