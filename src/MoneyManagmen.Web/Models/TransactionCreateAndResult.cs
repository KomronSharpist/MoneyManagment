using MoneyManagment.Service.DTOs.TransactionCategory;

namespace MoneyManagmen.Web.Models
{
    public class TransactionCreateAndResult
    {
        public List<TransactionCategoryResultDto> TransactionCategories { get; set; }
        public TransactionCategoryCreationDto CreationDto { get; set; }
    }
}
